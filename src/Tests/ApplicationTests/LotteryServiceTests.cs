
using Application.Configuration;
using Application.Services;
using Contracts.Factories;
using Contracts.Repositories;
using Contracts.Services;
using Domain.Entities;
using Domain.Entities.Players;
using FluentAssertions;
using Implementation.Factories;
using Implementation.Repositories;
using Moq;



namespace BedeLottery.Application.Tests.Services
{
    public class LotteryServiceTests
    {
        private readonly Mock<IGameRepository> _mockRepository;
        private readonly Mock<IPlayerFactory> _mockPlayerFactory;
        private readonly Mock<ITicketFactory> _mockTicketFactory;
        private readonly Mock<IRandomService> _mockRandomService;
        private readonly Mock<IPlayerInputService> _mockPlayerInputService;
        private readonly DefaultLotteryConfiguration _config;
        private readonly LotteryService _lotteryService;

        public LotteryServiceTests()
        {
            _mockRepository = new Mock<IGameRepository>();
            _mockPlayerFactory = new Mock<IPlayerFactory>();
            _mockTicketFactory = new Mock<ITicketFactory>();
            _mockRandomService = new Mock<IRandomService>();
            _mockPlayerInputService = new Mock<IPlayerInputService>();


            _config = new DefaultLotteryConfiguration
            {
                MinPlayers = 10,
                MaxPlayers = 15,
                StartingBalance = 10m,
                TicketPrice = 1m
            };

            _lotteryService = new LotteryService(
                _config,
                _mockRandomService.Object,
                _mockRepository.Object,
                _mockPlayerFactory.Object,
                _mockTicketFactory.Object);
        }



        [Fact]
        public void GetTotalRevenue_WithNoTickets_ShouldReturnZero()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllTickets()).Returns(new List<Ticket>());

            // Act
            var revenue = _lotteryService.GetTotalRevenue();

            // Assert
            revenue.Should().Be(0m);
        }


        [Fact]
        public void PurchaseTicket_ShouldAddTicket_AndReduceBalance()
        {
            var player = new HumanPlayer(100m, 1);
            var tickets = new List<Ticket>
            {
                new Ticket(1, player),
                new Ticket(2, player),
                new Ticket(3, player)
            };

            _mockTicketFactory.Setup(f => f.CreateMultipleTickets(player, 3))
                .Returns(tickets);

            // Act
            _lotteryService.PurchaseTicketsForPlayer(player, 3);

            // Assert
            player.Balance.Should().Be(97m); // 100 - (3 * 1)
        }




        [Fact]
        public void PurchaseTicketsForPlayer_ShouldThrowException_WhenInsufficientBalance()
        {
            // Arrange
            var player = new HumanPlayer(2m, 1);

            // Act
            Action act = () => _lotteryService.PurchaseTicketsForPlayer(player, 5);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*insufficient balance*");
        }

        [Fact]
        public void PurchaseTicketsForPlayer_ShouldCreateTickets_AndDeductBalance()
        {
            // Arrange
            var player = new HumanPlayer(10m, 1); // 10 balance
            var ticketCount = 5;
            var expectedCost = 5m; // 5 tickets * 1 = $5

            var mockTickets = new List<Ticket>
    {
        new Ticket(1, player),
        new Ticket(2, player),
        new Ticket(3, player),
        new Ticket(4, player),
        new Ticket(5, player)
    };

            _mockTicketFactory
                .Setup(f => f.CreateMultipleTickets(player, ticketCount))
                .Returns(mockTickets);

            // Act
            _lotteryService.PurchaseTicketsForPlayer(player, ticketCount);

            // Assert
            player.Balance.Should().Be(5m); // 10 - 5 = 5
            player.GetTickets().Should().HaveCount(5);
            _mockRepository.Verify(r => r.AddTickets(It.IsAny<List<Ticket>>()), Times.Once);
        }


        [Fact]
        public void GetTotalRevenue_ShouldCalculateCorrectly()
        {
            // Arrange
            var player1 = new HumanPlayer(100m, 1);
            var player2 = new CPUUser(100m, 1);

            var tickets = new List<Ticket>
    {
        new Ticket(1, player1),
        new Ticket(2, player1),
        new Ticket(3, player2)
    };

            _mockRepository
                .Setup(r => r.GetAllTickets())
                .Returns(tickets);

            // Act
            var revenue = _lotteryService.GetTotalPoolRevenue();

            // Assert
            revenue.Should().Be(3m); // 3 tickets * 1 = 3
        }


        [Fact]
        public void GetHouseProfit_AfterDrawingWinners_ShouldBePositive()
        {
            // Arrange
            var config = new DefaultLotteryConfiguration
            {
                MinPlayers = 3,
                MaxPlayers = 5,
                StartingBalance = 100m,
                TicketPrice = 1m,
                Prizes =
                new List<PrizeTierConfiguration> {new PrizeTierConfiguration
                {
                    Name = "Grand Prize",
                    RevenuePercentage = 50m // 50% of revenue to grand prize
                },
                 new PrizeTierConfiguration
                {
                    Name = "Second Tier",
                    RevenuePercentage = 30m, // 30% of revenue
                    TicketPercentage = 10m
                },
                new PrizeTierConfiguration
                {
                    Name = "Third Tier",
                    RevenuePercentage = 20m, // 20% of revenue
                    TicketPercentage = 15m
                }
            }
            };

            var realRepository = new InMemoryGameRepository();
            var realPlayerFactory = new PlayerFactory();
            var realTicketFactory = new TicketFactory();
            var realRandomService = new RandomService();

            var lotteryService = new LotteryService(
                config,
                realRandomService,
                realRepository,
                realPlayerFactory,
                realTicketFactory);

            var player1 = new HumanPlayer(100m, 1);
            var player2 = new CPUUser(100m, 1);
            var player3 = new CPUUser(100m, 1);

            realRepository.AddPlayer(player1);
            realRepository.AddPlayer(player2);
            realRepository.AddPlayer(player3);

            // Act - Purchase tickets
            lotteryService.PurchaseTicketsForPlayer(player1, 10);
            lotteryService.PurchaseTicketsForPlayer(player2, 10);
            lotteryService.PurchaseTicketsForPlayer(player3, 10);

            // Setup to credit revenue (30 tickets × 1 = 30)
            lotteryService.SetupTicketPurchases();

            var revenueBeforeDraw = lotteryService.GetTotalRevenue();
            Console.WriteLine($"Initial Revenue: ${revenueBeforeDraw}");

            // Draw winners, this should debits prizes from revenue
            lotteryService.DrawWinners();

            // Get results
            var prizesAwarded = lotteryService.GetTotalPrizesAwarded();
            var houseProfitAfterDraw = lotteryService.GetHouseProfit();

            // Debug output
            Console.WriteLine($"Prizes Awarded: ${prizesAwarded}");
            Console.WriteLine($"House Profit (Remaining Revenue): ${houseProfitAfterDraw}");
            Console.WriteLine($"Winners: {realRepository.GetAllWinners().Count()}");

            // Assert
            revenueBeforeDraw.Should().Be(30m); // 30 tickets × 1

            prizesAwarded.Should().BeGreaterThan(0m); // Some prizes were awarded

            // House profit = Remaining revenue after paying prizes
            houseProfitAfterDraw.Should().Be(revenueBeforeDraw - prizesAwarded);

            // House profit should always be positive 
            houseProfitAfterDraw.Should().BeGreaterThanOrEqualTo(0m);

        }


        [Fact]
        public void WhenAllPlayersBuy_TotalTicketsEqual100_DrawRunsAndHistoryIsPopulated()
        {
            // Arrange
            var ticketPrice = 1m;
            var totalPlayers = 14; // 1 human + 13 CPUs (match your sample)
            var totalTicketsWanted = 100;

            var config = new DefaultLotteryConfiguration
            {
                MinPlayers = totalPlayers,
                MaxPlayers = totalPlayers,
                StartingBalance = 10m,
                TicketPrice = ticketPrice,
                Prizes = new List<PrizeTierConfiguration>
                {
                    new PrizeTierConfiguration { Name = "Grand Prize", RevenuePercentage = 50m, FixedTicketCount = 1 },
                    new PrizeTierConfiguration { Name = "Second Tier", RevenuePercentage = 30m, TicketPercentage = 10m },
                    new PrizeTierConfiguration { Name = "Third Tier", RevenuePercentage = 10m, TicketPercentage = 20m },
                }
            };

            // concrete in-memory implementations (adjust if your types/names differ)
            var repository = new InMemoryGameRepository();
            var playerFactory = new PlayerFactory();
            var ticketFactory = new TicketFactory();
            var randomService = new RandomService(); // if it accepts seed, pass one for determinism

            var service = new LotteryService(
                config,
                randomService,
                repository,
                playerFactory,
                ticketFactory
            );

            // Create human player and initialize CPU players
            var human = new HumanPlayer(startingBalance: 10m, id: 1);
            service.InitializePlayers(human);

            var players = repository.GetAllPlayers().ToList();
            players.Should().HaveCount(totalPlayers);

            // Distribute tickets so total across all players equals totalTicketsWanted
            // Give the human a fixed number, then split remaining across CPUs
            int humanTickets = 1;
            int remaining = totalTicketsWanted - humanTickets;
            var cpuPlayers = players.Where(p => !p.IsHuman).ToList();
            int cpuCount = cpuPlayers.Count;

            int basePerCpu = remaining / cpuCount;
            int remainder = remaining % cpuCount;

            // Assign tickets
            human.TicketCount = humanTickets;

            for (int i = 0; i < cpuCount; i++)
            {
                int assign = basePerCpu + (i < remainder ? 1 : 0); // distribute remainder to first N CPUs
                cpuPlayers[i].TicketCount = assign;
            }

            // sanity: sum = 100
            var sum = repository.GetAllPlayers().Sum(p => p.TicketCount);
            sum.Should().Be(totalTicketsWanted);

            // Act: create tickets and credit revenue
            service.SetupTicketPurchases();

            // Sanity checks before draw
            var totalTickets = repository.GetAllTickets().Count;
            totalTickets.Should().Be(totalTicketsWanted);

            var revenueBeforeDraw = service.GetTotalRevenue();
            revenueBeforeDraw.Should().Be(totalTicketsWanted * ticketPrice);

            // Draw winners
            service.DrawWinners();

            // Collect results
            var prizesAwarded = service.GetTotalPrizesAwarded();
            var houseProfitAfterDraw = service.GetHouseProfit();
            var history = service.GetGameHistory().ToList();

            // Assertions
            prizesAwarded.Should().BeGreaterThan(0m, "prizes should be awarded when ticket revenue/prize percentages are configured.");
            houseProfitAfterDraw.Should().Be(revenueBeforeDraw - prizesAwarded);
            houseProfitAfterDraw.Should().BeGreaterThanOrEqualTo(0m);

            // History should contain each configured prize (names)
            history.Select(h => h.PrizeName).Should().BeEquivalentTo(new[] { "Grand Prize", "Second Tier", "Third Tier" });

            // Each prize group should have at least one winning ticket (subject to strategy logic)
            foreach (var g in history)
            {
                g.Tickets.Should().NotBeNull();
                g.Tickets.Count.Should().BeGreaterThanOrEqualTo(1);
                g.Owners.Should().NotBeNull();
                g.Owners.Count.Should().BeGreaterThanOrEqualTo(1);
            }

            // Human should be present among players
            var humanFromRepo = repository.GetHumanPlayer();
            humanFromRepo.Should().NotBeNull();
            humanFromRepo!.IsHuman.Should().BeTrue();
        }
    }
}
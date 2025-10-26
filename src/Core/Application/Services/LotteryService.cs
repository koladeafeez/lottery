

using Application.Configuration;
using Contracts.Factories;
using Contracts.Models;
using Contracts.Repositories;
using Contracts.Services;
using Domain.Entities;
using Domain.Entities.Players;
using Domain.Entities.Prizes;
using Domain.Enums;
using System.Numerics;

namespace Application.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly DefaultLotteryConfiguration _config;
        private readonly IRandomService _randomService;
        private readonly IGameRepository _repository;
        private readonly IPlayerFactory _playerFactory;
        private readonly ITicketFactory _ticketFactory;
        private readonly List<IGameEventListener> _listeners;
        private decimal _ticketPrice;

        public LotteryService(
            DefaultLotteryConfiguration config,
            IRandomService randomService,
            IGameRepository repository,
            IPlayerFactory playerFactory,
            ITicketFactory ticketFactory)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _randomService = randomService ?? throw new ArgumentNullException(nameof(randomService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
            _ticketFactory = ticketFactory ?? throw new ArgumentNullException(nameof(ticketFactory));
            _listeners = new List<IGameEventListener>();


            _ticketPrice = _config.TicketPrice;
            InitializePrizes();
        }

        public void AddListener(IGameEventListener listener)
        {
            _listeners.Add(listener);
        }


        private void NotifyGameStarted()
        {
            foreach (var listener in _listeners)
                listener.OnGameStarted(_repository.GetAllPlayers());
        }

        private void NotifyPlayerCreated(Player player)
        {
            foreach (var listener in _listeners)
                listener.OnPlayerCreated(player);
        }

        private void NotifyTicketPurchased(Player player, int ticketCount, decimal cost)
        {
            foreach (var listener in _listeners)
                listener.OnTicketPurchased(player, ticketCount, cost);
        }

        private void NotifyDrawStarted()
        {
            foreach (var listener in _listeners)
                listener.OnDrawStarted();
        }

        private void NotifyWinnerDrawn(IEnumerable<WinnerInfo> winners)
        {
            foreach (var listener in _listeners)
                listener.OnWinnerDrawn(winners);
        }

        private void NotifyDrawCompleted()
        {
            foreach (var listener in _listeners)
                listener.OnDrawCompleted(_repository.GetAllWinners());
        }



        public IEnumerable<PrizeHistory> GetGameHistory()
        {
            var allWinners = _repository.GetAllWinners();

            var history = allWinners
                .Where(w => w.Prize != null && w.Ticket != null)
                .GroupBy(w => w.Prize!.Name)
                .Select(group => new PrizeHistory
                {
                    PrizeName = group.Key,
                    PrizeWon = group.First().PrizeWon,
                    Tickets = group.Select(w => w.Ticket!).ToList(),
                    Owners = group.Select(w => w.Ticket!.Owner).Distinct().ToList()
                })
                .ToList();

            return history;
        }

        public decimal GetTicketPrice() => _ticketPrice;

        private void InitializePrizes()
        {

            foreach (var prize in _config.Prizes)
            {
                if (prize.TicketPercentage == decimal.Zero)
                {
                    _repository.AddPrize(new GrandPrize(
                    prize.Name, prize.Order, prize.FixedTicketCount,  prize.RevenuePercentage));
                }
                else
                {
                    _repository.AddPrize(new TierPrize(
                    prize.Name,
                    prize.Order,
                    prize.RevenuePercentage,
                    prize.TicketPercentage));
                }
            }
        }

        public void InitializePlayers(Player humanPlayer)
        {

            // Add human player
            _repository.AddPlayer(humanPlayer);
            NotifyPlayerCreated(humanPlayer);

            // Create CPU players
            int totalPlayers = _randomService.Next(_config.MinPlayers, _config.MaxPlayers + 1);
            int cpuCount = totalPlayers - 1;

            var cpuPlayers = _playerFactory.CreateMultiplePlayers(
                PlayerTypeEnum.CPU,
                cpuCount,
                _config.StartingBalance);

            foreach (var cpu in cpuPlayers)
            {
                _repository.AddPlayer(cpu);
                NotifyPlayerCreated(cpu);
            }

        }

        public PurchaseTicketResult SellTickets()
        {
            var humanPlayer = _repository.GetHumanPlayer();
            if (humanPlayer == null)
                return new PurchaseTicketResult { Message = "Human player not found", IsSuccess = false };

            if (humanPlayer.TicketCount <= 0)
                return new PurchaseTicketResult { Message = "No tickets purchased by human", IsSuccess = false };

            // Handle CPU ticket purchases
            PurchaseTicketsForCPUPlayers();

            // Add tickets to game
            SetupTicketPurchases();

            return new PurchaseTicketResult { Message = "Tickets sold successfully", IsSuccess = true };
        }

        private void PurchaseTicketsForCPUPlayers()
        {
            var cpuPlayers = _repository.GetAllPlayers().Where(p => !p.IsHuman);
            foreach (var cpu in cpuPlayers)
            {
                int ticketCount = _randomService.Next(_config.MinTicketsPerPlayer, _config.MaxTicketsPerPlayer + 1);

                // Calculate cost
                decimal cost = ticketCount * _config.TicketPrice;

                // Check if they can afford it, if not, buy what they can afford
                if (cost > cpu.Balance)
                {
                    ticketCount = (int)(cpu.Balance / _config.TicketPrice);
                }

                cpu.TicketCount = ticketCount;
            }
        }

        public void PurchaseTicketsForPlayer(Player player, int ticketCount)
        {
            if (ticketCount <= 0) return;

            decimal cost = ticketCount * _ticketPrice;
            if (cost > player.Balance)
                throw new InvalidOperationException("Insufficient balance");

            var tickets = _ticketFactory.CreateMultipleTickets(player, ticketCount);

            // Add to player and repository
            player.SetTickets(tickets);
            _repository.AddTickets(tickets);

            // Deduct balance
            player.UpdateBalance(-cost);

            NotifyTicketPurchased(player, ticketCount, cost);
        }

        public void UpdateRevenueWithTicketCount(int count)
        {
            _repository.CreditRevenue(count * _ticketPrice);
        }

        public void SetupTicketPurchases()
        {
            var count = 0;
            var jjj = _repository.GetAllPlayers();
            foreach (var player in _repository.GetAllPlayers())
            {
                PurchaseTicketsForPlayer(player, player.TicketCount);
                count += player.GetTickets().Count();
            }
            _repository.CreditRevenue(count * _ticketPrice);
            NotifyGameStarted();
        }

        public void DrawWinners()
        {

            NotifyDrawStarted();
            _repository.ClearWinners();

            var drawContext = CreateDrawContext();
            ProcessAllPrizeTiers(drawContext);

            NotifyDrawCompleted();
        }

        private DrawContext CreateDrawContext()
        {
            return new DrawContext
            {
                AvailableTickets = new List<Ticket>(_repository.GetAllTickets()),
                TotalRevenue = _repository.GetTotalRevenue(),
                TotalTicketCount = _repository.GetAllTickets().Count
            };
        }

        private void ProcessAllPrizeTiers(DrawContext context)
        {
            foreach (var prize in _repository.GetAllPrizes().OrderBy(x => x.Priority))
            {
                ProcessPrizeTier(prize, context);
            }
        }


        private void ProcessPrizeTier(Prize prize, DrawContext context)
        {
            var drawnTickets = DrawPrizeTier(
                context.AvailableTickets,
                prize,
                context.TotalRevenue,
                context.TotalTicketCount);

        }

        private IList<Ticket> DrawMultipleTickets(List<Ticket> availableTickets, int numberOfDraws)
        {
            var ticketsDrawn = new List<Ticket>();
            for (int i = 0; i < numberOfDraws; i++)
                ticketsDrawn.Add(DrawRandomTicket(availableTickets));

            return ticketsDrawn;
        }

        private WinnerInfo UpdateTicketWinner(Ticket ticket, Prize prize, decimal prizePerTicket)
        {
            decimal balanceBefore = ticket.Owner.Balance;

            CreditWinner(ticket.Owner, prizePerTicket);

            var winnerInfo = new WinnerInfo
            {
                Ticket = ticket,
                Prize = prize,
                PrizeWon = prizePerTicket,
                BalanceBefore = balanceBefore,
                BalanceAfter = ticket.Owner.Balance
            };
            _repository.AddWinner(winnerInfo);

            return winnerInfo;

        }


        private void CreditWinner(Player player, decimal prize)
        {
            player.UpdateBalance(prize);
            _repository.DebitRevenue(prize);

        }

        private IList<Ticket> DrawPrizeTier(List<Ticket> availableTickets, Prize prize, decimal totalRevenue, int totalTicketCount)
        {
            var drawnedTickets = new List<Ticket>();
            if (availableTickets.Count == 0) 
                return drawnedTickets;



            int winnerCount = prize.GetWinnerCount(totalTicketCount);


            decimal prizePerTicket = prize.GetPrizePerTicket(totalRevenue, totalTicketCount);


            if (winnerCount > 1) // Multiple winners
            {
                var winningTickets = DrawMultipleTickets(availableTickets, winnerCount);
                drawnedTickets.AddRange(winningTickets);
                var winnerInfo = new List<WinnerInfo>();
                foreach (var winningTicket in winningTickets)
                {
                    winningTicket.PrizeWon = prizePerTicket;
                    var updateInfo = UpdateTicketWinner(winningTicket, prize, prizePerTicket);
                    winnerInfo.Add(updateInfo);
                }

                NotifyWinnerDrawn(winnerInfo);
            }
            else
            {
                var winningTicket = DrawRandomTicket(availableTickets);
                winningTicket.PrizeWon = prizePerTicket;
                drawnedTickets.Add(winningTicket);
                var updateInfo = UpdateTicketWinner(winningTicket, prize, prizePerTicket);
                NotifyWinnerDrawn(new List<WinnerInfo> { updateInfo });

            }
            return drawnedTickets;

        }

        private Ticket DrawRandomTicket(List<Ticket> availableTickets)
        {
            int index = _randomService.Next(0, availableTickets.Count());
            var ticket = availableTickets[index];
            availableTickets.RemoveAt(index);
            return ticket;
        }


        public void ResetGame()
        {
            _repository.Clear();

            _playerFactory.ResetCounter(1);
            Ticket.ResetIdCounter();
        }


        public IReadOnlyList<Player> GetPlayers() => _repository.GetAllPlayers();
        public IReadOnlyList<Ticket> GetAllTickets() => _repository.GetAllTickets();
        public IEnumerable<Prize> GetPrizes() => _repository.GetAllPrizes();

        public decimal GetTotalPrizesAwarded() => _repository.GetWinningTickets().Sum(t => t.PrizeWon);

        public decimal GetHouseProfit() => _repository.GetTotalRevenue();

        public decimal GetTotalRevenue() => _repository.GetTotalRevenue();

        public decimal GetTotalPoolRevenue()
        {
            var tickets = _repository.GetAllTickets();
            return tickets.Count * _config.TicketPrice;
        }

    }

    public class DrawContext
    {
        public List<Ticket> AvailableTickets { get; set; } = new List<Ticket>();
        public decimal TotalRevenue { get; set; }
        public int TotalTicketCount { get; set; }
    }


}


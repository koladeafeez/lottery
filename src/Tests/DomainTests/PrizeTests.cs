
using Contracts.Strategies;
using Domain.Entities.Prizes;
using Domain.Entities.PrizeStrategies;
using FluentAssertions;
using Moq;
using Xunit;

namespace BedeLottery.Domain.Tests.Entities
{
    public class PrizeTests
    {

        [Fact]
        public void GetPrizePool_WithFixedWinnerStrategy_ReturnsExpectedPrizePool()
        {
            // Arrange
            // revenuePercentage is used directly by the current FixedWinnerStrategy implementation.
            // Use 0.5m to represent 50% in the current code.
            var prize = new GrandPrize("Grand Prize", fixedTicketCount: 1, revenuePercentage: 50m);

            // Act
            var prizePool = prize.GetPrizePool(1000m);

            // Assert
            prizePool.Should().Be(500m);
        }


        [Fact]
        public void GetWinnerCount_WithFixedWinnerStrategy_ReturnsFixedCount_WhenTicketsPresent()
        {
            // Arrange
            var prize = new GrandPrize("Grand Prize", fixedTicketCount: 2, revenuePercentage: 0.1m);

            // Act
            var winnerCount = prize.GetWinnerCount(100);

            // Assert
            winnerCount.Should().Be(2);
        }

        [Fact]
        public void GetWinnerCount_WithFixedWinnerStrategy_ReturnsZero_WhenNoTickets()
        {
            // Arrange
            var prize = new GrandPrize("Grand Prize", fixedTicketCount: 2, revenuePercentage: 0.1m);

            // Act
            var winnerCount = prize.GetWinnerCount(0);

            // Assert
            winnerCount.Should().Be(0);
        
    }


        [Fact]
        public void GetPrizePool_FixedWinnerStrategy_ShouldReturn_200()
        {
            // Arrange
            var s =new FixedWinnerStrategy(1, 20m);

            // Act
            var prizePool = s.CalculatePrizePool(1000m);

            // Assert
            prizePool.Should().Be(200m);
        }

        [Fact]
        public void GetPrizePool_PercentageWinnerStrategy_ShouldReturn_300()
        {
            // Arrange
            var s = new PercentageWinnerStrategy(10, 30m);

            // Act
            var prizePool = s.CalculatePrizePool(1000m);

            // Assert
            prizePool.Should().Be(300m);
        }

        [Fact]
        public void GetPrizePerTicket_ShouldReturnZero_WhenNoWinners()
        {
            // Arrange
            var mockStrategy = new Mock<IPrizeDistributionStrategy>();
            mockStrategy.Setup(s => s.CalculateWinnerCount(0)).Returns(0);
            var prize = new GrandPrize("Grand Prize", 1, 20);

            // Act
            var prizePerTicket = prize.GetPrizePerTicket(1000m, 0);

            // Assert
            prizePerTicket.Should().Be(0m);
        }

        [Theory]
        [InlineData(30.00, 7, 4.28)]   // 30 / 7 = 4.2857 -> 4.28
        [InlineData(100.00, 3, 33.33)] // 100 / 3 = 33.3333 -> 33.33
        [InlineData(50.00, 4, 12.50)]  // 50 / 4 = 12.5 -> 12.50
        public void GetPrizePerTicket_ShouldRoundCorrectly(decimal totalRevenue, int totalTickets, decimal expected)
        {
            // Arrange
            // Make revenuePercentage = 100 so prizePool = totalRevenue
            // and fixedWinnerCount = totalTickets so winnerCount = totalTickets
            var prize = new GrandPrize("Grand Prize", totalTickets, 100m);

            // Act
            var prizePerTicket = prize.GetPrizePerTicket(totalRevenue, totalTickets);

            // Assert
            prizePerTicket.Should().Be(expected);
        }

    }
}
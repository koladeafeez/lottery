using Domain.Entities.Players;
using FluentAssertions;
using Xunit;
using Domain.Entities;

namespace DomainTests
{
        public class PlayerTests
        {
            [Fact]
            public void HumanPlayer_ShouldInitialize_WithCorrectValues()
            {
                // Arrange & Act
                var player = new HumanPlayer(100m, 1);

                // Assert
                player.Id.Should().Be(1);
                player.Balance.Should().Be(100m);
                player.GetTickets().Should().BeEmpty();
   
            }

            [Fact]
            public void CPUUser_ShouldInitialize_WithCorrectValues()
            {
                // Arrange & Act
                var player = new CPUUser(50m, 2);

                // Assert
                player.Id.Should().Be(2);
                player.Balance.Should().Be(50m);
                player.GetTickets().Should().BeEmpty();
            }

            [Fact]
            public void Balance_ShouldThrowException_WhenSetToNegative()
            {
                // Arrange
                var player = new HumanPlayer(100m, 1);

                // Act
                Action act = () => player.UpdateBalance(-200m);

                // Assert
                act.Should().Throw<InvalidOperationException>();
            }

     

            [Fact]
            public void WinPrize_ShouldIncreaseBalance()
            {
                // Arrange
                var player = new HumanPlayer(100m, 1);

                // Act
                player.UpdateBalance(50m);

                // Assert
                player.Balance.Should().Be(150m);
            }

            [Fact]
            public void WinningTicketCount_ShouldReturnCorrectCount()
            {
                // Arrange
                var player = new HumanPlayer(100m, 1);
                var ticket1 = new Ticket(1, player) { PrizeWon = 10m };
                var ticket2 = new Ticket(2, player) { PrizeWon = 0m };
                var ticket3 = new Ticket(3, player) { PrizeWon = 20m };

                player.SetTickets(new List<Ticket> { ticket1, ticket2, ticket3 });

                // Act
                var winningCount = player.WinningTicketCount;

                // Assert
                winningCount.Should().Be(2);
            }

            [Fact]
            public void TotalWinnings_ShouldReturnIncrementBalance()
            {
                // Arrange
                var player = new HumanPlayer(100m, 1);
                var ticket1 = new Ticket(1, player) { PrizeWon = 10m };
                var ticket2 = new Ticket(2, player) { PrizeWon = 20m };
                var ticket3 = new Ticket(3, player) { PrizeWon = 30m };
                var tickets = new List<Ticket> { ticket1, ticket2, ticket3 };

            player.SetTickets(tickets);

            // Act
                var totalWinnings = player.Balance + tickets.Sum(x => x.PrizeWon);

                // Assert
                totalWinnings.Should().Be(160m);
            }
        }
}

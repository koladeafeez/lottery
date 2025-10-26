
using Domain.Entities;
using Domain.Entities.Players;
using FluentAssertions;
using Xunit;

namespace BedeLottery.Domain.Tests.Entities
{
    public class TicketTests
    {
        [Fact]
        public void Ticket_ShouldInitialize_WithCorrectValues()
        {
            // Arrange
            var player = new HumanPlayer(100m, 1);

            // Act
            var ticket = new Ticket(1, player);

            // Assert
            ticket.Id.Should().Be(1);
            ticket.Owner.Should().Be(player);
            ticket.PrizeWon.Should().Be(0m);
            ticket.IsWinner.Should().BeFalse();
        }

        [Fact]
        public void Ticket_ShouldThrowException_WhenOwnerIsNull()
        {
            // Act
            Action act = () => new Ticket(1, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void IsWinner_ShouldReturnTrue_WhenPrizeWonIsGreaterThanZero()
        {
            // Arrange
            var player = new HumanPlayer(100m, 1);
            var ticket = new Ticket(1, player);

            // Act
            ticket.PrizeWon = 50m;

            // Assert
            ticket.IsWinner.Should().BeTrue();
        }

        [Fact]
        public void IsWinner_ShouldReturnFalse_WhenPrizeWonIsZero()
        {
            // Arrange
            var player = new HumanPlayer(100m, 1);
            var ticket = new Ticket(1, player);

            // Assert
            ticket.IsWinner.Should().BeFalse();
        }

        [Fact]
        public void CreateMultiple_ShouldCreateCorrectNumberOfTickets()
        {
            // Arrange
            var player = new HumanPlayer(100m, 1);

            // Act
            var tickets = Ticket.CreateMultiple(player, 5);

            // Assert
            tickets.Should().HaveCount(5);
            tickets.Should().OnlyContain(t => t.Owner == player);
        }

        [Fact]
        public void CreateMultiple_ShouldThrowException_WhenCountIsZeroOrNegative()
        {
            // Arrange
            var player = new HumanPlayer(100m, 1);

            // Act
            Action act = () => Ticket.CreateMultiple(player, 0);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateMultiple_ShouldThrowException_WhenOwnerIsNull()
        {
            // Act
            Action act = () => Ticket.CreateMultiple(null, 5);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
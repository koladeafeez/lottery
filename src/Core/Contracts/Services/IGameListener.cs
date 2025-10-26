using Contracts.Models;
using Domain.Entities.Players;


namespace Contracts.Services
{
    public interface IGameEventListener
    {
        void OnGameStarted(IEnumerable<Player> players);
        void OnPlayerCreated(Player player);
        void OnTicketPurchased(Player player, int ticketCount, decimal cost);
        void OnDrawStarted();
        void OnWinnerDrawn(IEnumerable<WinnerInfo> winners);
        void OnDrawCompleted(IEnumerable<WinnerInfo> winners);
        void OnGameCompleted(decimal totalRevenue, decimal totalPrizes, decimal houseProfit);
    }
}

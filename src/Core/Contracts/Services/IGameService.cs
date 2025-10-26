using Contracts.Models;
using Domain.Entities.Players;


namespace Contracts.Services
{
    public interface IGameService
    {
        void GeneratePlayers();
        PurchaseTicketResult SellTickets();
        (decimal, IEnumerable<PrizeHistory>) PublishGameResults();
        void InitializeGame(HumanPlayer humanPlayer);
        bool StartGame();
        decimal GetTicketPrice();
        decimal GetHouseProfit();
    }
}

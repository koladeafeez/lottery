using Domain.Entities.Players;


namespace Contracts.Services        
{
    public interface IPlayerInputHandler
    {
        int GetTicketPurchaseCount(Player player, int minTickets, int maxTickets, decimal ticketPrice);

    }
}

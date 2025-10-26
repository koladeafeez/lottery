
using Contracts.Models;
using Domain.Entities;
using Domain.Entities.Players;
using Domain.Entities.Prizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services
{
    public interface ILotteryService
    {
        void AddListener(IGameEventListener listener);
        void DrawWinners();
        IReadOnlyList<Ticket> GetAllTickets();
        decimal GetHouseProfit();
        IReadOnlyList<Player> GetPlayers();
        IEnumerable<Prize> GetPrizes();
        decimal GetTotalPrizesAwarded();
        void InitializePlayers(Player humanPlayer);
        void PurchaseTicketsForPlayer(Player player, int ticketCount);
        void ResetGame();
        void SetupTicketPurchases();

        decimal GetTotalRevenue();
        decimal GetTotalPoolRevenue();
        void UpdateRevenueWithTicketCount(int count);
        PurchaseTicketResult SellTickets();
        decimal GetTicketPrice();
        IEnumerable<PrizeHistory> GetGameHistory();
    }
}

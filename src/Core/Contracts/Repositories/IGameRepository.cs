
using Contracts.Models;
using Domain.Entities;
using Domain.Entities.Players;
using Domain.Entities.Prizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IGameRepository
    {
        // Player
        void AddPlayer(Player player);
        void AddPlayers(IEnumerable<Player> players);
        IReadOnlyList<Player> GetAllPlayers();
        Player GetPlayerById(int id);
        Player GetHumanPlayer();

        // Ticket 
        void AddTicket(Ticket ticket);
        void AddTickets(IEnumerable<Ticket> tickets);
        IReadOnlyList<Ticket> GetAllTickets();
        IEnumerable<Ticket> GetWinningTickets();
        IEnumerable<Ticket> GetTicketsByPlayer(int playerId);

        // Prize
        void AddPrize(Prize prize);
        IEnumerable<Prize> GetAllPrizes();

        // Winner 
        void AddWinner(WinnerInfo winner);
        IEnumerable<WinnerInfo> GetAllWinners();
        void ClearWinners();

        // Game state
        void Clear();
        int GetTotalPlayers();
        int GetTotalTickets();
        void CreditRevenue(decimal amount);
        void DebitRevenue(decimal amount);
        decimal GetTotalRevenue();
    }
}

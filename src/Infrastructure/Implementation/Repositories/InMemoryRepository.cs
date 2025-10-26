
using Contracts.Models;
using Contracts.Repositories;
using Domain.Entities;
using Domain.Entities.Players;
using Domain.Entities.Prizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Repositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        public readonly List<Game> _games = new();
        private readonly List<Player> _players = new();
        private readonly List<Ticket> _tickets = new();
        private readonly List<Prize> _prizes = new();
        private readonly List<WinnerInfo> _winners = new();
        private readonly object _lock = new object();
        private decimal _totalRevenue = 0;

        // Player
        public void AddPlayer(Player player)
        {
            lock (_lock)
            {
                _players.Add(player);
            }
        }

        public void AddPlayers(IEnumerable<Player> players)
        {
            lock (_lock)
            {
                _players.AddRange(players);
            }
        }

        public IReadOnlyList<Player> GetAllPlayers() => _players.AsReadOnly();

        public IReadOnlyList<Game> GetHostory() => _games.AsReadOnly();

        public Player GetPlayerById(int id) => _players.FirstOrDefault(p => p.Id == id);

        public Player GetHumanPlayer() => _players.FirstOrDefault(p => p.IsHuman);

        public decimal GetTotalRevenue() => _totalRevenue;


        public void CreditRevenue(decimal amount)
        { 
            lock (_lock)
            {
                _totalRevenue += amount;
            }
        }


        public void DebitRevenue(decimal amount)
        {
            lock (_lock)
            {
                _totalRevenue -= amount;
            }
        }


        public void AddGame(Game game)
        {
            lock (_lock)
            {
                _games.Add(game);
            }
        }

        // Ticket
        public void AddTicket(Ticket ticket)
        {
            lock (_lock)
            {
                _tickets.Add(ticket);
            }
        }

        public void AddTickets(IEnumerable<Ticket> tickets)
        {
            lock (_lock)
            {
                _tickets.AddRange(tickets);
            }
        }

        public IReadOnlyList<Ticket> GetAllTickets() => _tickets.AsReadOnly();

        public IEnumerable<Ticket> GetWinningTickets() => _tickets.Where(t => t.IsWinner);

        public IEnumerable<Ticket> GetTicketsByPlayer(int playerId)
            => _tickets.Where(t => t.Owner.Id == playerId);

        // Prize
        public void AddPrize(Prize prize)
        {
            lock (_lock)
            {
                _prizes.Add(prize);
            }
        }

        public IEnumerable<Prize> GetAllPrizes() => _prizes;

        // Winner
        public void AddWinner(WinnerInfo winner)
        {
            lock (_lock)
            {
                _winners.Add(winner);
            }
        }

        public IEnumerable<WinnerInfo> GetAllWinners() => _winners;

        public void ClearWinners()
        {
            lock (_lock)
            {
                _winners.Clear();
            }
        }

        // Game
        public void Clear()
        {
            lock (_lock)
            {
                _players.Clear();
                _tickets.Clear();
                _winners.Clear();
                _totalRevenue = 0;
                _winners.Clear();
            }
        }

        public int GetTotalPlayers() => _players.Count;

        public int GetTotalTickets() => _tickets.Count;
    }
}

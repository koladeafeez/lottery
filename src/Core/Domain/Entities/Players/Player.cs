using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Players
{
    public abstract class Player
    {

        //private string _name = string.Empty;
        private static readonly object _lock = new object();
        public int Id { get; }

        //private int _gameCount;
        public int TicketCount { get; set; } = 0;

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;      
        }

        public void UpdateBalance(decimal amount)
        {
            if(_balance + amount < 0)
                throw new InvalidOperationException("Insufficient balance for this operation.");
           
            _balance += amount;
        }


        private List<Ticket> Tickets { get; } = new List<Ticket>();


        public string GetName()
        {
           return $"Player {Id}";

        }
        public void SetTickets(List<Ticket> tickets)
        {
            Tickets.Clear();
            Tickets.AddRange(tickets);
        }

        public IEnumerable<Ticket> GetTickets()
        {
           return Tickets;
        }

        protected Player(decimal startingBalance, int nextId)
        {
            lock (_lock)
            {
                Id = nextId;
            }
            UpdateBalance(startingBalance);
        }

        public int WinningTicketCount => Tickets.Count(t => t.IsWinner);


        public abstract bool IsHuman { get; }

        public abstract string GetPlayerType();

    }
}

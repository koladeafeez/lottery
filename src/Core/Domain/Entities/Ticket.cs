using System;
using System.Collections.Generic;
using Domain.Entities.Players;


namespace Domain.Entities
{
    public class Ticket
    {
        private static int _nextId = 1;
        private static readonly object _lock = new object();

        public int Id { get; }
        public Player Owner { get; }
        public decimal PrizeWon { get; set; }

        public Ticket(Player owner)
        {
            lock (_lock)
            {
                Id = _nextId++;
            }
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            PrizeWon = 0;
        }

        // For testing purposes - can inject specific ID
        public Ticket(int id, Player owner)
        {
            Id = id;
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            PrizeWon = 0;
        }

        public bool IsWinner => PrizeWon > 0;


        public static List<Ticket> CreateMultiple(Player owner, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be positive", nameof(count));

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            var tickets = new List<Ticket>(count);
            for (int i = 0; i < count; i++)
            {
                tickets.Add(new Ticket(owner));
            }
            return tickets;
        }


        public static void ResetIdCounter()
        {
            lock (_lock)
            {
                _nextId = 1;
            }
        }
    }
}

using Contracts.Factories;
using Domain.Entities;
using Domain.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Factories
{
    public class TicketFactory : ITicketFactory
    {
        private static int _nextId = 1;
        private static readonly object _lock = new object();

        public Ticket CreateTicket(Player owner)
        {
            lock (_lock)
            {
                return new Ticket(_nextId++, owner);
            }
        }

        public List<Ticket> CreateMultipleTickets(Player owner, int count)
        {
            var tickets = new List<Ticket>();
            for (int i = 0; i < count; i++)
            {
                tickets.Add(CreateTicket(owner));
            }
            return tickets;
        }
    }
}

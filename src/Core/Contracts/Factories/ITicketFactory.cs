
using Domain.Entities;
using Domain.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Factories
{
    public interface ITicketFactory
    {
        Ticket CreateTicket(Player owner);
        List<Ticket> CreateMultipleTickets(Player owner, int count);
    }
}

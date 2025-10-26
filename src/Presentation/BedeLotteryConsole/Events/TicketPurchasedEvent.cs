using Domain.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsoleUI.Events
{
    public class TicketPurchasedEvent
    {
        public Player Player { get; }
        public int TicketCount { get; }
        public decimal Cost { get; }

        public TicketPurchasedEvent(Player player, int ticketCount, decimal cost)
        {
            Player = player;
            TicketCount = ticketCount;
            Cost = cost;
        }
    }
}

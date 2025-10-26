using Domain.Entities.PrizeStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Prizes
{
    public class TierPrize : Prize
    {
        public decimal TicketPercentage { get; }

        public TierPrize(string name, int priority, decimal revenuePercentage, decimal ticketPercentage)
            : base(name, new PercentageWinnerStrategy(ticketPercentage, revenuePercentage), priority)
        {
            TicketPercentage = ticketPercentage;
        }
    }
}

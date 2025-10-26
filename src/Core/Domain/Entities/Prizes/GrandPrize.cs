
using Contracts.Strategies;
using Domain.Entities.PrizeStrategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Prizes
{
    public class GrandPrize : Prize
    {
        public GrandPrize(string name, int priority, int fixedTicketCount, decimal revenuePercentage)
            : base(name, new FixedWinnerStrategy(fixedTicketCount, revenuePercentage), priority)
        {
        }
    }
}

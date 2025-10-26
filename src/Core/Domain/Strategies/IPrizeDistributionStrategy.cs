using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Strategies
{
    public interface IPrizeDistributionStrategy
    {
        int CalculateWinnerCount(int totalTickets);
        decimal CalculatePrizePool(decimal totalRevenue);
    }
}

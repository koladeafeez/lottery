
using Contracts.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PrizeStrategies
{
    public class FixedWinnerStrategy : IPrizeDistributionStrategy
    {
        private readonly int _fixedWinnerCount;
        private readonly decimal _revenuePercentage;

        public FixedWinnerStrategy(int fixedWinnerCount, decimal revenuePercentage)
        {
            _fixedWinnerCount = fixedWinnerCount;
            _revenuePercentage = revenuePercentage/100m;
        }

        public int CalculateWinnerCount(int totalTickets)
        {
            return totalTickets > 0 ? _fixedWinnerCount : 0;
        }

        public decimal CalculatePrizePool(decimal totalRevenue)
        {
            return totalRevenue * _revenuePercentage;
        }
    }
}

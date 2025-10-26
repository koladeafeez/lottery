

using Contracts.Strategies;

namespace Domain.Entities.PrizeStrategies
{
    public class PercentageWinnerStrategy : IPrizeDistributionStrategy
    {
        private readonly decimal _ticketPercentage;
        private readonly decimal _revenuePercentage;

        public PercentageWinnerStrategy(decimal ticketPercentage, decimal revenuePercentage)
        {
            _ticketPercentage = ticketPercentage/100m;
            _revenuePercentage = revenuePercentage/100m;
        }

        public int CalculateWinnerCount(int totalTickets)
        {
            if (totalTickets == 0) 
                return 0;

            int count = (int)Math.Round(totalTickets * _ticketPercentage);
            return count;
        }

        public decimal CalculatePrizePool(decimal totalRevenue)
        {
            return totalRevenue * _revenuePercentage;
        }
    }
}

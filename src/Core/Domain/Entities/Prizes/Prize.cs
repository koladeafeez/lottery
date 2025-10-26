using Contracts.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Prizes
{

    public class Prize
    {
        public string Name { get; protected set; }

        public int Priority { get; protected set; }
        public IPrizeDistributionStrategy Strategy { get; protected set; }

        public Prize(string name, IPrizeDistributionStrategy strategy, int priority)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            Priority = priority;
        }

        public int GetWinnerCount(int totalTickets)
        {
            return Strategy.CalculateWinnerCount(totalTickets);
        }

        public decimal GetPrizePool(decimal totalRevenue)
        {
            return Strategy.CalculatePrizePool(totalRevenue);
        }

        public decimal GetPrizePerTicket(decimal totalRevenue, int totalTickets)
        {
            int winnerCount = GetWinnerCount(totalTickets);
            if (winnerCount == 0) return 0m;

            decimal prizePool = GetPrizePool(totalRevenue);
            decimal exactAmount = prizePool / winnerCount;

            decimal prizePerTicket = Math.Truncate(exactAmount * 100) / 100;

            return prizePerTicket;
        }


    }

}



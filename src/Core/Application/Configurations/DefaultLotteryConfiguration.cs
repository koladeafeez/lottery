using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class DefaultLotteryConfiguration
    {
        public int PlayerStartCount { get; set; } = 1;
        public int MinPlayers { get; set; } = 10;
        public int MaxPlayers { get; set; } = 15;
        public int MinTicketsPerPlayer { get; set; } = 1;
        public int MaxTicketsPerPlayer { get; set; } = 10;
        public decimal StartingBalance { get; set; } = 10m;
        public decimal TicketPrice { get; set; } = 1m;

        public List<PrizeTierConfiguration> Prizes { get; set; } = new();

        public List<PrizeTierConfiguration> GetPrizeTierConfigurations()
        {
            var GrandPrize = new PrizeTierConfiguration
            {
                Name = "Grand Prize",
                TicketPercentage = 0m,
                RevenuePercentage = 50,
                IsFixedTicketCount = true,
                FixedTicketCount = 1,
                Order = 1
            };

            var SecondTier = new PrizeTierConfiguration
            {
                Name = "Second Tier",
                TicketPercentage = 10,
                RevenuePercentage = 30,
                Order = 2
            };

            var ThirdTier = new PrizeTierConfiguration
            {
                Name = "Third Tier",
                TicketPercentage = 20,
                RevenuePercentage = 10,
                Order = 3
            };
            return new List<PrizeTierConfiguration>
            {
                GrandPrize,
                SecondTier,
                ThirdTier
            };
        }


        public (bool, string) Validate()
        {
            if (MinPlayers <= 0 || MaxPlayers < MinPlayers)
                return (false, "Invalid player count configuration");

            if (MinTicketsPerPlayer <= 0 || MaxTicketsPerPlayer < MinTicketsPerPlayer)
                return (false, "Invalid ticket count configuration");

            if (StartingBalance < TicketPrice * MinTicketsPerPlayer)
                return (false, "Starting balance must allow at least minimum ticket purchase");

            if (TicketPrice <= 0)
                return (false, "Ticket price must be positive");

            decimal totalPrizePercentage = Prizes.Sum(x => x.RevenuePercentage);

            if (totalPrizePercentage > 100m)
                return (false, "Total prize percentages cannot exceed 100%");

            bool isOrderUnique = Prizes.Select(t => t.Order).Distinct().Count() == Prizes.Count;
            if (!isOrderUnique)
                return (false, "Prize Order Must be Unique and Ascending order");

            return (true, "valid");
        }
    }

    public class PrizeTierConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public decimal TicketPercentage { get; set; }
        public decimal RevenuePercentage { get; set; }
        public bool IsFixedTicketCount { get; set; }
        public int FixedTicketCount { get; set; }
        public int Order
        {
            get; set;
        }
    }
}


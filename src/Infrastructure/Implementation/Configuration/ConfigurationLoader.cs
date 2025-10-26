using Application.Configuration;
using Microsoft.Extensions.Configuration;




namespace Implementation.Configuration
{
    public class ConfigurationLoader
    {
        public static DefaultLotteryConfiguration LoadConfiguration(string fileName = "appsettings.json")
        {
            try
            {
  
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Warning: {fileName} not found. Using default configuration.");
                    return new DefaultLotteryConfiguration();
                }


                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

                // Create and bind the main configuration
                var lotteryConfig = new DefaultLotteryConfiguration();
                configuration.GetSection("DefaultLotteryConfiguration").Bind(lotteryConfig);


                // Validate configuration
                try
                {
                    var result = lotteryConfig.Validate();
                    if (result.Item1 == true)
                    {
                        Console.WriteLine($"Configuration loaded successfully from {fileName}");
                        return lotteryConfig;
                    }else
                    { 
                        Console.WriteLine($"Warning: Configuration validation failed: {result.Item2}");
                        Console.WriteLine("Using default configuration.");
                        return new DefaultLotteryConfiguration();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Configuration validation failed: {ex.Message}");
                    Console.WriteLine("Using default configuration.");
                    return new DefaultLotteryConfiguration() { Prizes = new DefaultLotteryConfiguration().GetPrizeTierConfigurations()};
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                Console.WriteLine("Using default configuration.");
                return new DefaultLotteryConfiguration();
            }
        }

        public static void DisplayConfiguration(DefaultLotteryConfiguration config)
        {
            Console.WriteLine("\n=== Game Configuration ===");
            Console.WriteLine($"Players: {config.MinPlayers}-{config.MaxPlayers}");
            Console.WriteLine($"Tickets per player: {config.MinTicketsPerPlayer}-{config.MaxTicketsPerPlayer}");
            Console.WriteLine($"Starting balance: ${config.StartingBalance:F2}");
            Console.WriteLine($"Ticket price: ${config.TicketPrice:F2}");
            Console.WriteLine("\nPrize Distribution:");
            foreach (var prize in config.Prizes)
            {
                if (prize.IsFixedTicketCount)
                {
                    Console.WriteLine($"  {prize.Name}: {prize.FixedTicketCount} ticket(s) win {prize.RevenuePercentage:P0} of revenue");
                }
                else
                {
                    Console.WriteLine($"  {prize.Name}: {prize.TicketPercentage:P0} of tickets share {prize.RevenuePercentage:P0} of revenue");
                }
            }
            Console.WriteLine();
        }
    }
}

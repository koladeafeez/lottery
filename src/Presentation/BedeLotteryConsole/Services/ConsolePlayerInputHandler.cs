using Contracts.Services;
using Domain.Entities.Players;


namespace BedeLotteryConsoleUI.Services
{
    public class ConsolePlayerInputHandler : IPlayerInputHandler
    {
        private readonly IUserInterface _ui;
   
        public ConsolePlayerInputHandler(IUserInterface ui)
        {
            _ui = ui;

        }


        private (bool, string) ValidateTicketPurchaseInput(int count, int min, int max, decimal balance, decimal price)
        {
            if (count >= min && count <= max)
            {
                decimal cost = count * price;
                if (cost <= balance)
                {
                    return ((true, $"."));
                }
                else
                {
                    return ((false, $"Insufficient balance. You can afford up to {(int)(balance / price)} tickets."));
                }
            }
            else
            {
                return (false, $"Please enter a number between {min} and {max}.");
            }

        }
       

        public int GetTicketPurchaseCount(
            Player player,
            int minTickets,
            int maxTickets,
            decimal ticketPrice)
        {
            while (true)
            {
                _ui.Write($"How many tickets would you like to purchase, Player 1? " +
                         $"({minTickets}-{maxTickets}, ${ticketPrice} each, Balance: ${player.Balance:F2}): ");

                string input = _ui.ReadLine();

                if (int.TryParse(input, out int count))
                {
                    var validationResult = ValidateTicketPurchaseInput(count,minTickets,maxTickets, player.Balance, ticketPrice);

                    if (validationResult.Item1 == true)
                    {
                        return count;
                    }
                    
                    _ui.WriteLine($"Invalid input: {validationResult.Item2}");
                }
                else
                {
                    _ui.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

        }
    }
}

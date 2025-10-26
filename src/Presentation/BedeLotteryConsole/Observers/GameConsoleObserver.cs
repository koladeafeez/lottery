

using Contracts.Models;
using Contracts.Services;
using Domain.Entities.Players;
using System.Text;


namespace BedeLotteryConsole.Observers
{
        public class GameConsoleObserver : IGameEventListener
        {

        
            public void OnGameStarted(IEnumerable<Player> players)
            {
            System.Console.WriteLine("--------------Game Status -------------------");
            foreach (var d in players)
                Console.WriteLine($"{d.GetName()}: Name : {d.GetPlayerType()} : tickets {d.GetTickets().Count()} , Balance : {d.Balance}");
            System.Console.WriteLine("-----------------------------------------------");
        }

            public void OnPlayerCreated(Player player)
            {
            Console.WriteLine($"{player.GetName()} is created with {player.TicketCount} ticket(s) request");
            }

            public void OnTicketPurchased(Player player, int ticketCount, decimal cost)
            {


            }

            public void OnDrawStarted()
            {
                Console.WriteLine("\n🎲 Drawing winners...");
            }

        public void OnWinnerDrawn(IEnumerable<WinnerInfo> winners)
        {
            if (winners.Count() > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{winners.First().Prize.Name}: Players ");
                foreach (var winner in winners.GroupBy(x => x.Ticket.Owner.Id))
                {
                    sb.Append($"{winner.Key}({winner.Count()}), ");
                }
                sb.Remove(sb.Length - 2, 1);
                sb.Append($"win ${winners.First().Ticket.PrizeWon} per winning ticket!");
                Console.WriteLine(sb.ToString());
            }
            else
            {
                var winner = winners.First();
                Console.WriteLine($"{winner.Prize.Name}:{winner.Ticket.Owner.GetName()}(TicketId-{winner.Ticket.Id}) win ${winner.Ticket.PrizeWon:F2}");
            }
        }

            public void OnDrawCompleted(IEnumerable<WinnerInfo> winners)
            {
                Console.WriteLine($"✓ Draw completed! {winners.Count()} winner(s) drawn.");
            }

            public void OnGameCompleted(decimal totalRevenue, decimal totalPrizes, decimal houseProfit)
            {
                Console.WriteLine($"\n💰 Game completed - Revenue: ${totalRevenue:F2}, Prizes: ${totalPrizes:F2}, Profit: ${houseProfit:F2}");
            }
        }
    }

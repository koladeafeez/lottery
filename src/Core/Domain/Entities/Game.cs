using Domain.Entities.Players;
using Domain.Entities.Prizes;


namespace Domain.Entities
{
    public class Game
    {
        public List<Player> Players { get; }
        public List<Ticket> Tickets { get; }
        public List<Prize> Prizes { get; }
        public decimal TicketPrice { get; }

        public Game(decimal ticketPrice)
        {
            TicketPrice = ticketPrice;
            Players = new List<Player>();
            Tickets = new List<Ticket>();
            Prizes = new List<Prize>();
        }


    }
}

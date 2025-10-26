using Domain.Entities;
using Domain.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    public class PrizeHistory
    {
        public string PrizeName { get; set; } = string.Empty;
        public List<Ticket> Tickets { get; set; } = new();
        public decimal PrizeWon { get; set; }
        public List<Player> Owners { get; set; } = new();
    }
}

using Domain.Entities;
using Domain.Entities.Prizes;


namespace Contracts.Models
{
    public class WinnerInfo
    {
        public Ticket? Ticket { get; set; }
        public Prize? Prize { get; set; }
        public decimal PrizeWon { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}

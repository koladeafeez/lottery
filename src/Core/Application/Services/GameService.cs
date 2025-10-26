using Contracts.Models;
using Contracts.Services;
using Domain.Entities.Players;

namespace Application.Services
{
    public class GameService : IGameService
    {

        private readonly ILotteryService _lotteryService;
        public GameService(ILotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }

        public void GeneratePlayers()
        {
            throw new NotImplementedException();
        }

        public void InitializeGame(HumanPlayer humanPlayer)
        {
            // Set All Players
            _lotteryService.InitializePlayers(humanPlayer);
        }

        public decimal GetTicketPrice()
        {
            return _lotteryService.GetTicketPrice();
        }

        public (decimal, IEnumerable<PrizeHistory>) PublishGameResults()
        {
            var result = _lotteryService.GetGameHistory();
            var houseProfit = _lotteryService.GetHouseProfit();

            return (houseProfit, result);
        }


        public decimal GetHouseProfit()
        {
            return _lotteryService.GetHouseProfit();
        }

        public PurchaseTicketResult SellTickets()
        {
            return _lotteryService.SellTickets();
        }

        public bool StartGame()
        {

            if (!ValidateGameState())
                return false;

            _lotteryService.DrawWinners();

            return true;
        }

        public bool ValidateGameState()
        {
            var players = _lotteryService.GetPlayers();
            var tickets = _lotteryService.GetAllTickets();

            if (players.Count == 0 || !players.Any(x => x.IsHuman))
                return false;

            if (tickets.Count == 0)
                return false;

            if (_lotteryService.GetPrizes().Count() == 0)
                return false;

            return true;
        }

    }
}

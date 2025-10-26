using Application.Configuration;
using Contracts.Services;
using Domain.Entities.Players;
using System.ComponentModel.DataAnnotations;

namespace Application.Services
{

    public class HumanPlayerService : IHumanPlayerInputService
    {
        private readonly DefaultLotteryConfiguration _config;
        private readonly IPlayerManagementService _playerManagement;
        private readonly IPlayerInputHandler _inputHandler;

        public HumanPlayerService(
            DefaultLotteryConfiguration config,
            IPlayerManagementService playerManagement,
            IPlayerInputHandler inputHandler)
        {
            _config = config;
            _playerManagement = playerManagement;
            _inputHandler = inputHandler;
        }

        public HumanPlayer GetPlayer()
        {
            return _playerManagement.GetCurrentPlayer();
        }


        public void ClearPlayer()
        {
            _playerManagement.ResetPlayer();
        }

        public void InitializeHumanPlayer()
        {
            var player = new HumanPlayer(_config.StartingBalance, _config.PlayerStartCount);
            var ticketCount = SetupPlayerForGame(player);
            player.TicketCount = ticketCount;

            _playerManagement.SetCurrentPlayer(player);
        }

        private int SetupPlayerForGame(HumanPlayer player)
        {
             
            int ticketCount = _inputHandler.GetTicketPurchaseCount(player,
                _config.MinTicketsPerPlayer,
                _config.MaxTicketsPerPlayer,
                _config.TicketPrice);

            return ticketCount;
        }

    }
    
}

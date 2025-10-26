using Application.Configuration;
using Contracts.Factories;
using Contracts.Services;
using Domain.Entities.Players;
using Domain.Enums;


namespace Application.Services
{

    public class PlayerManagementService : IPlayerManagementService
    {
        private HumanPlayer _currentPlayer;
        private readonly IPlayerFactory _playerFactory;
        private readonly DefaultLotteryConfiguration _config;

        public PlayerManagementService(
            IPlayerFactory playerFactory,
            DefaultLotteryConfiguration config)
        {
            _playerFactory = playerFactory;
            _config = config;

            _currentPlayer = (HumanPlayer)_playerFactory.CreatePlayer(
                PlayerTypeEnum.Human,
                _config.StartingBalance);
        }

        public HumanPlayer GetCurrentPlayer() => _currentPlayer;

        public void SetCurrentPlayer(HumanPlayer player)
        {

            _currentPlayer = player ?? throw new ArgumentNullException(nameof(player));
        }

        public void ResetPlayer()
        {
            _currentPlayer = null;
        }

    }
}

using Domain.Entities.Players;


namespace Contracts.Services
{
    public interface IPlayerManagementService
    {
        HumanPlayer GetCurrentPlayer();
        void SetCurrentPlayer(HumanPlayer player);
        void ResetPlayer();
    }
}

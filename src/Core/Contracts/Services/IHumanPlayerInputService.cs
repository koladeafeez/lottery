using Domain.Entities.Players;


namespace Contracts.Services
{
    public interface IHumanPlayerInputService
    {
        HumanPlayer GetPlayer();
        void InitializeHumanPlayer();

    }
}

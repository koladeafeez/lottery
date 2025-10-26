



using BedeLotteryConsoleUI.Services;
using Contracts.Services;
using Domain.Entities;

namespace BedeLotteryConsole.Views
{
    public class PreGameView : ViewBase
    {
        private readonly IHumanPlayerInputService _inputService;
        private IConsoleService _consoleService;

        public PreGameView(
            IHumanPlayerInputService inputService,
            IConsoleService consoleService 
            )
        {
            _inputService = inputService;
            _consoleService = consoleService;   
        }

        public override void Run()
        {
            _consoleService.ShowWelcome();

            _inputService.InitializeHumanPlayer();

            _consoleService.InitializeGame();

            _consoleService.SellTickets();

        }

    }
}
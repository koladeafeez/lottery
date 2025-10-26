

using BedeLotteryConsoleUI.Services;
using Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsole.Views
{
    public class GameView : ViewBase
    {
        private readonly IConsoleService _consoleService;
        public GameView(
            IConsoleService consoleService
            )
        {
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        }

        public override void Run()
        {
            _consoleService.StartGame();

        }

    }
}

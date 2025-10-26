using BedeLotteryConsoleUI.Services;
using Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsole.Views
{
    public class PlayAgainView : ViewBase
    {
        private readonly Action<bool> _onComplete;
        private readonly IConsoleService _consoleService;
        public PlayAgainView(IConsoleService consoleService, Action<bool> onComplete)
        {
           _consoleService = consoleService;
            _onComplete = onComplete;
        }

        public override void Run()
        {
            var status = _consoleService.PromptPlayerToPlayAgain();
            _onComplete?.Invoke(status);
        }
    }
}

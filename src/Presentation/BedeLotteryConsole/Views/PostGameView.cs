

using BedeLotteryConsoleUI.Services;
using Contracts.Services;
using Domain.Entities.Prizes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsole.Views
{
    public class PostGameView : ViewBase
    {
        private readonly IConsoleService _consoleService;

        public PostGameView(
            IConsoleService consoleService)
        {
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        }

        public override void Run()
        {

            _consoleService.DisplayResults();
        }

    }
}


using Contracts.Models;
using Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsoleUI.Services
{

    public interface IConsoleService
    {
        void SellTickets();
        void ShowWelcome();

        void InitializeGame();
        void StartGame();

        void DisplayResults();

        bool PromptPlayerToPlayAgain();
    }
    public class ConsoleService : IConsoleService
    {
        private readonly IGameService _gameService;
        private readonly IUserInterface _ui;
        private readonly IHumanPlayerInputService _inputService;
        public ConsoleService(IGameService gameService, IUserInterface uI, IHumanPlayerInputService humanPlayerInputService)
        {
            _gameService = gameService;
            _ui = uI;
            _inputService = humanPlayerInputService;
        }


        public bool PromptPlayerToPlayAgain()
        {
          _ui.WriteLine("Play Again?");

            _ui.WriteLine("Would you like to play another game?");
            _ui.WriteLine("\n1. Yes - Play again");
            _ui.WriteLine("2. No - Exit");
            _ui.WriteLine("\nSelect an option: ");

            var input = _ui.ReadLine();
            bool continueGame = input == "1";

            if (continueGame)
            {
                _ui.WriteLine("\nStarting new game...");
                Thread.Sleep(1000);
            }
            else
            {
                _ui.WriteLine("\nThank you for playing!");
                Thread.Sleep(1000);
            }

            return continueGame;


        }

        public void ShowWelcome()
        {
            _ui.WriteLine($"** Your Digital balance : {_inputService.GetPlayer().Balance}\n");
            Console.WriteLine($"** Ticket Price : ${_gameService.GetTicketPrice()}\n");
        }

        public void InitializeGame()
        {
            // Set All Players
            _gameService.InitializeGame(_inputService.GetPlayer());

        }

        public void SellTickets()
        {
            var result = _gameService.SellTickets();

            if (result.IsSuccess)
            {
                _ui.WriteLine(result.Message);
            }
            else
            {
                _ui.Write(result.Message);
            }
        }

        public void StartGame()
        {
            // validate game state
            var isGameStarted = _gameService.StartGame();

           if(!isGameStarted)
            {
            _ui.WriteLine("Game could not be started due to invalid state.");
            }

        }


        public void DisplayResults()
        {

            _ui.WriteLine("Congratulations to the Winners!");
            var houseProfit = _gameService.GetHouseProfit();
            _ui.WriteLine($"\nHouse Profit: ${houseProfit:F2}");
        }
    }
}

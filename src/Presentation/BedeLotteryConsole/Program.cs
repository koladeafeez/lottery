

using BedeLotteryConsole.Configurations;
using BedeLotteryConsole.Extensions;
using BedeLotteryConsole.Observers;
using BedeLotteryConsole.Views;
using BedeLotteryConsoleUI.Extensions;
using BedeLotteryConsoleUI.Services;
using Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using System;



var serviceProvider = new ServiceCollection()
    .AddServices()
    .BuildServiceProvider();


using var scope = serviceProvider.CreateScope();
var services = scope.ServiceProvider;
var lotteryService = services.GetRequiredService<ILotteryService>();

var playerInputService = services.GetRequiredService<IHumanPlayerInputService>();

var consoleService = services.GetRequiredService<IConsoleService>();
var consoleLogger = services.GetRequiredService<GameConsoleObserver>();

lotteryService.AddListener(consoleLogger);




Console.WriteLine("╔════════════════════════════════════════╗");
Console.WriteLine("║   LOTTERY GAME - Console Edition       ║");
Console.WriteLine("╚════════════════════════════════════════╝\n");
System.Console.WriteLine("=== Welcome to the Bede Lottery Player 1 === \n");


bool continuePlaying = true;

while (continuePlaying)
{



    var views = new ViewCollection()

         .ConfigureViews(new PreGameView(playerInputService, consoleService))
        .ConfigureViews(new GameView(consoleService))
        .ConfigureViews(new PostGameView(consoleService))
         .ConfigureViews(new PlayAgainView(consoleService, shouldContinue =>
         {
             continuePlaying = shouldContinue;
         }));

    views.RunAll();

    if (continuePlaying)
    {
        lotteryService.ResetGame();
        Console.Clear();
    }


}




Console.WriteLine("\n╔════════════════════════════════════════╗");
Console.WriteLine("║   Thanks for playing!                  ║");
Console.WriteLine("╚════════════════════════════════════════╝");
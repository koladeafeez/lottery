using BedeLotteryConsole.Observers;
using BedeLotteryConsoleUI.Services;
using Contracts.Factories;
using Contracts.Repositories;
using Contracts.Services;
using Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BedeLotteryConsoleUI.Extensions
    {
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddLotteryServices();

            services.AddScoped<GameConsoleObserver>();

            services.AddScoped<IUserInterface, ConsoleUserInterface>();

            // Services
            services.AddScoped<IPlayerInputHandler, ConsolePlayerInputHandler>();
            services.AddScoped<IConsoleService, ConsoleService>();

            return services;
        }
    }
}

using Application.Configuration;
using Application.Services;
using Contracts.Factories;
using Contracts.Repositories;
using Contracts.Services;
using Implementation.Configuration;
using Implementation.Factories;
using Implementation.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace Implementation
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddLotteryServices(this IServiceCollection services)
        {
            services.AddSingleton<DefaultLotteryConfiguration>(sp =>
            {
                var config = ConfigurationLoader.LoadConfiguration();
                ConfigurationLoader.DisplayConfiguration(config);
                return config;
            });


            services.AddSingleton<IRandomService, RandomService>();
            services.AddSingleton<IPlayerManagementService, PlayerManagementService>();
            services.AddScoped<IHumanPlayerInputService, HumanPlayerService>();

            services.AddScoped<IPlayerFactory, PlayerFactory>();
            services.AddSingleton<ITicketFactory, TicketFactory>();

            services.AddTransient<IGameRepository, InMemoryGameRepository>();
            services.AddScoped<ILotteryService, LotteryService>();

            services.AddScoped<IGameService, GameService>();

            return services;
        }
    }
}

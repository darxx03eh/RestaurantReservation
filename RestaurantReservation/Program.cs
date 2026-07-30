using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Dependencies;
using RestaurantReservation.UI;
using RestaurantReservation.UI.Dependencies;

namespace RestaurantReservation;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            var configuration = LoadConfigurations();
            var services = new ServiceCollection();
            services.AddRestaurantReservationDbContext(configuration)
                    .AddRestaurantReservationDbContextFactory(configuration)
                    .AddInfrastructureDependencies()
                    .AddUiDependencies();
            var provider = services.BuildServiceProvider();
            var console = provider.GetRequiredService<RestaurantReservationConsole>();
            await console.RunAsync();
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Application failed to start.");
            Console.ResetColor();
            Console.Error.WriteLine(exception.GetBaseException().Message);
        }
    }
    private static IConfiguration LoadConfigurations() 
    => new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
}

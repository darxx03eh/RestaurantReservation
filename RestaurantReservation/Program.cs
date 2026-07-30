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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var services = new ServiceCollection();
            services.AddDbContext<RestaurantReservationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("RestaurantReservationDbLocalConnection"));
            });
            services.AddDbContextFactory<RestaurantReservationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("RestaurantReservationDbLocalConnection"));
            });
            services.AddUiDependencies()
                .AddInfrastructureDependencies();
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
}

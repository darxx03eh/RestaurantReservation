using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RestaurantReservation.Infrastructure.Db;

public class RestaurantReservationDbContextFactory : IDesignTimeDbContextFactory<RestaurantReservationDbContext>
{
    public RestaurantReservationDbContext CreateDbContext(string[] args)
    {
        var configuration = LoadConfigurationsForFactory();

        var optionsBuilder = new DbContextOptionsBuilder<RestaurantReservationDbContext>();

        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString(
                "RestaurantReservationDbLocalConnection"));

        return new RestaurantReservationDbContext(optionsBuilder.Options);
    }
    private static IConfiguration LoadConfigurationsForFactory()
    => new ConfigurationBuilder()
        .SetBasePath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "../RestaurantReservation"))
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
}
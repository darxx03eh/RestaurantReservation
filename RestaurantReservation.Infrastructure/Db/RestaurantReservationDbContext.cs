using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RestaurantReservation.Infrastructure.Db;

public class RestaurantReservationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public RestaurantReservationDbContext() => _configuration = LoadConfigurations();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("RestaurantReservationDbLocalConnection"));

    private IConfiguration LoadConfigurations()
        => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
}
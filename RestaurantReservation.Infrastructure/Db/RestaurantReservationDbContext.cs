using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Db;

public class RestaurantReservationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public RestaurantReservationDbContext() => _configuration = LoadConfigurations();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("RestaurantReservationDbLocalConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    private IConfiguration LoadConfigurations()
        => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<OrderItem>  OrderItems { get; set; }
}
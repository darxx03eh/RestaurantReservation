using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

namespace RestaurantReservation.Infrastructure.Seeders;

public class SeederRunner
{
    private readonly List<ISeeder> _seeders;

    public SeederRunner() => _seeders =
    [
        new CustomerSeeder(), new RestaurantSeeder(), new EmployeeSeeder(),
        new MenuItemSeeder(), new TableSeeder(), new ReservationSeeder(),
        new OrderSeeder(), new OrderItemSeeder()
    ];

    public async Task RunAsync(RestaurantReservationDbContext context)
    {
        foreach (var seeder in _seeders)
            await seeder.SeedAsync(context);
    }
}
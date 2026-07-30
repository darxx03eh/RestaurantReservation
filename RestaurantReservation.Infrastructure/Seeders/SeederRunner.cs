using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

namespace RestaurantReservation.Infrastructure.Seeders;

public sealed class SeederRunner
{
    private readonly IDbContextFactory<RestaurantReservationDbContext> _contextFactory;
    private readonly IEnumerable<ISeeder> _seeders;
    public SeederRunner(IEnumerable<ISeeder> seeders, IDbContextFactory<RestaurantReservationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
        _seeders = seeders.OrderBy(x => x.Order);
    }

    public async Task RunAsync(RestaurantReservationDbContext context)
    {
        foreach (var seeder in _seeders)
            await seeder.SeedAsync(context);
    }
    
    public async Task<bool> IsAlreadySeededAsync()
    {
        var tasks = new[]
        {
            CheckTableAsync(db => db.Customers.AnyAsync()),
            CheckTableAsync(db => db.Employees.AnyAsync()),
            CheckTableAsync(db => db.MenuItems.AnyAsync()),
            CheckTableAsync(db => db.OrderItems.AnyAsync()),
            CheckTableAsync(db => db.Orders.AnyAsync()),
            CheckTableAsync(db => db.Reservations.AnyAsync()),
            CheckTableAsync(db => db.Restaurants.AnyAsync()),
            CheckTableAsync(db => db.Tables.AnyAsync())
        };

        var results = await Task.WhenAll(tasks);

        return results.All(x => x);
    }

    private async Task<bool> CheckTableAsync(
        Func<RestaurantReservationDbContext, Task<bool>> query)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await query(context);
    }
}
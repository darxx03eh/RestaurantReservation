using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders;

public interface ISeeder
{
    int Order { get; }
    Task SeedAsync(RestaurantReservationDbContext context);
}
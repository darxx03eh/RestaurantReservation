using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders;

public interface ISeeder
{
    Task SeedAsync(RestaurantReservationDbContext context);
}
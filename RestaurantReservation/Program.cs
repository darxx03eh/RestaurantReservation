using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Seeders;

namespace RestaurantReservation;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new RestaurantReservationDbContext();
        var runner = new SeederRunner();
        await runner.RunAsync(context);
    }
}
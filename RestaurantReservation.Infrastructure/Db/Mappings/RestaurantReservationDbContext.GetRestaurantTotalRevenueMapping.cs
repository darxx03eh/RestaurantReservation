using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.Infrastructure.Db;

public partial class RestaurantReservationDbContext
{
    private void ConfigureGetRestaurantTotalRevenueFunctions(ModelBuilder modelBuilder)
    => modelBuilder.HasDbFunction(
        typeof(RestaurantReservationDbContext)
            .GetMethod(nameof(GetRestaurantTotalRevenue), new[] { typeof(int) })!)
        .HasName("fn_GetRestaurantTotalRevenue");
}
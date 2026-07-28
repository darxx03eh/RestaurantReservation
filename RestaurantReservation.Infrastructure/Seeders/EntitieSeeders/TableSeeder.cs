using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public sealed class TableSeeder : ISeeder
{
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfTable = await context.Tables.CountAsync();
        if (numberOfTable <= 0)
        {
            var tables = new List<Table>()
            {
                new Table()
                {
                    RestaurantId = 1,
                    Capacity = 4
                },
                new Table()
                {
                    RestaurantId = 2,
                    Capacity = 4
                },
                new Table()
                {
                    RestaurantId = 3,
                    Capacity = 6
                },
                new Table()
                {
                    RestaurantId = 4,
                    Capacity = 4
                },
                new Table()
                {
                    RestaurantId = 5,
                    Capacity = 4
                },
            };
            await context.BulkInsertAsync(tables);
        }
    }
}
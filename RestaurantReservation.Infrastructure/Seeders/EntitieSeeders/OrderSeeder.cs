using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public sealed class OrderSeeder : ISeeder
{
    public int Order => 7;
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfOrders = await context.Orders.CountAsync();
        if (numberOfOrders <= 0)
        {
            var orders = new List<Order>()
            {
                new Order()
                {
                    ReservationId = 1,
                    EmployeeId = 1,
                    OrderDate = new DateTime(2026, 8, 1, 18, 15, 0),
                    TotalAmount = 72.53m
                },
                new Order()
                {
                    ReservationId = 2,
                    EmployeeId = 2,
                    OrderDate = new DateTime(2026, 8, 2, 19, 15, 0),
                    TotalAmount = 72.95m
                },
                new Order()
                {
                    ReservationId = 3,
                    EmployeeId = 3,
                    OrderDate = new DateTime(2026, 8, 3, 20, 15, 0),
                    TotalAmount = 110.58m
                },
                new Order()
                {
                    ReservationId = 4,
                    EmployeeId = 4,
                    OrderDate = new DateTime(2026, 8, 4, 18, 45, 0),
                    TotalAmount = 112.30m
                },
                new Order()
                {
                    ReservationId = 5,
                    EmployeeId = 5,
                    OrderDate = new DateTime(2026, 8, 5, 19, 45, 0),
                    TotalAmount = 143.49m
                },
            };
            await context.BulkInsertAsync(orders);
        }
    }
}
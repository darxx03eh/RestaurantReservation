using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public class OrderItemSeeder : ISeeder
{
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfOrderItems = await context.OrderItems.CountAsync();
        if (numberOfOrderItems <= 0)
        {
            var orderItems = new List<OrderItem>()
            {
                new OrderItem()
                {
                    OrderId = 1,
                    ItemId = 1,
                    Quantity = 2
                },
                new OrderItem()
                {
                    OrderId = 2,
                    ItemId = 2,
                    Quantity = 1
                },
                new OrderItem()
                {
                    OrderId = 2,
                    ItemId = 3,
                    Quantity = 3
                },
                new OrderItem()
                {
                    OrderId = 3,
                    ItemId = 4,
                    Quantity = 2
                },
                new OrderItem()
                {
                    OrderId = 4,
                    ItemId = 5,
                    Quantity = 1
                },
            };
            await context.BulkInsertAsync(orderItems);
        }
    }
}
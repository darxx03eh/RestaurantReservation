using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public class MenuItemSeeder : ISeeder
{
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfItems = await context.MenuItems.CountAsync();
        if (numberOfItems <= 0)
        {
            var items = new List<MenuItem>()
            {
                new MenuItem()
                {
                    RestaurantId = 1,
                    Name = "Bibimbap",
                    Description = "Rice bowl with vegetables, beef, and a fried egg",
                    Price = 15.21m
                },
                new MenuItem()
                {
                    RestaurantId = 1,
                    Name = "Kunafa",
                    Description = "Sweet cheese pastry soaked in sugar syrup",
                    Price = 6.20m
                },
                new MenuItem()
                {
                    RestaurantId = 1,
                    Name = "Cola",
                    Description = "Chilled classic cola",
                    Price = 4.03m
                },
                new MenuItem()
                {
                    RestaurantId = 1,
                    Name = "Fresh Lemonade",
                    Description = "Freshly squeezed lemonade with mint",
                    Price = 4.57m
                },
                new MenuItem()
                {
                    RestaurantId = 1,
                    Name = "Dumplings",
                    Description = "Steamed dumplings filled with pork and cabbage",
                    Price = 8.89m
                }
            };
            await context.BulkInsertAsync(items);
        }
    }
}
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public sealed class RestaurantSeeder : ISeeder
{
    public int Order => 2;
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfRestaurants = await context.Restaurants.CountAsync();
        if (numberOfRestaurants <= 0)
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "Rustic Spoon",
                    Address = "433 Jill Springs, New Roberttown, CO 29158",
                    PhoneNumber = "+393218196",
                    OpeningHours = "10:00 - 22:00"
                },
                new Restaurant()
                {
                    Name = "The Golden Kitchen",
                    Address = "386 Shane Harbors, Port Lindachester, MA 36922",
                    PhoneNumber = "+33908386379",
                    OpeningHours = "09:00 - 21:30"
                },
                new Restaurant()
                {
                    Name = "The Golden House",
                    Address = "16155 Roman Stream Suite 816, New Kellystad, OK 25704",
                    PhoneNumber = "+97042351161",
                    OpeningHours = "09:00 - 21:30"
                },
                new Restaurant()
                {
                    Name = "Amber Corner",
                    Address = "341 Michelle Light, Shawnstad, GA 49021",
                    PhoneNumber = "+6181618495",
                    OpeningHours = "08:00 - 20:00"
                },
                new Restaurant()
                {
                    Name = "Olive Kitchen",
                    Address = "192 Frank Light Suite 835, East Lydiamouth, MO 35594",
                    PhoneNumber = "+814131647",
                    OpeningHours = "17:00 - 23:30"
                }
            };
            await context.BulkInsertAsync(restaurants);
        }
    }
}
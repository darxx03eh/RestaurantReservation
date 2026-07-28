using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public class CustomerSeeder : ISeeder
{
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfCustomers = await context.Customers.CountAsync();
        if (numberOfCustomers <= 0)
        {
            var customers = new List<Customer>()
            {
                new Customer()
                {
                    FirstName = "Brandon",
                    LastName = "Flynn",
                    Email = "brandon.flynn22@yahoo.com",
                    PhoneNumber = "+8190278742"
                },
                new Customer()
                {
                    FirstName = "Michael",
                    LastName = "Jordan",
                    Email = "michael.jordan600@gmail.com",
                    PhoneNumber = "+970717565512"
                },
                new Customer()
                {
                    FirstName = "Amanda",
                    LastName = "Knight",
                    Email = "amanda.knight338@gmail.com",
                    PhoneNumber = "+970746807154"
                },
                new Customer()
                {
                    FirstName = "George",
                    LastName = "Miller",
                    Email = "george.miller332@yahoo.com",
                    PhoneNumber = "+4480876038"
                },
                new Customer()
                {
                    FirstName = "Christine",
                    LastName = "Allen",
                    Email = "christine.allen371@hotmail.com",
                    PhoneNumber = "+9170348247"
                }
            };
            await context.BulkInsertAsync(customers);
        }
    }
}
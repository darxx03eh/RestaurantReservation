using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public sealed class EmployeeSeeder : ISeeder
{
    public int Order => 3;
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfEmployees = await context.Employees.CountAsync();
        if (numberOfEmployees <= 0)
        {
            var employees = new List<Employee>()
            {
                new Employee()
                {
                    RestaurantId = 1,
                    FirstName = "Kathy",
                    LastName = "Obrien",
                    Position = Position.Manager
                },
                new Employee()
                {
                    RestaurantId = 2,
                    FirstName = "Krystal",
                    LastName = "Perez",
                    Position = Position.Cashier
                },
                new Employee()
                {
                    RestaurantId = 3,
                    FirstName = "Alexandra",
                    LastName = "Hayes",
                    Position = Position.VipOrdersWaiter
                },
                new Employee()
                {
                    RestaurantId = 4,
                    FirstName = "Scott",
                    LastName = "Dalton",
                    Position = Position.Chef
                },
                new Employee()
                {
                    RestaurantId = 5,
                    FirstName = "Veronica",
                    LastName = "Torres",
                    Position = Position.AssistantWaiter
                }
            };
            await context.BulkInsertAsync(employees);
        }
    }
}
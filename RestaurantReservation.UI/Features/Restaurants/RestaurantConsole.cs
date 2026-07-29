using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.UI.Common;
using Spectre.Console;

namespace RestaurantReservation.UI.Features.Restaurants;

internal sealed class RestaurantConsole(RestaurantReservationDbContext context, IUnitOfWork unitOfWork)
{
    public async Task ListAsync()
    {
        var restaurants = await unitOfWork.Restaurants.GetAllAsync();
        if (Ui.ShowEmptyIfNeeded(restaurants, "restaurants"))
            return;

        var table = Ui.CreateTable("Restaurants", "Id", "Name", "Address", "Phone", "Hours");

        foreach (var restaurant in restaurants.OrderBy(restaurant => restaurant.RestaurantId))
        {
            table.AddRow(
                restaurant.RestaurantId.ToString(),
                restaurant.Name,
                restaurant.Address,
                restaurant.PhoneNumber ?? "",
                restaurant.OpeningHours ?? "");
        }

        AnsiConsole.Write(table);
    }

    public async Task ShowRevenueAsync()
    {
        var rows = await context.Restaurants
            .AsNoTracking()
            .Select(restaurant => new
            {
                restaurant.RestaurantId,
                restaurant.Name,
                Revenue = context.GetRestaurantTotalRevenue(restaurant.RestaurantId)
            })
            .OrderBy(row => row.RestaurantId)
            .ToListAsync();

        if (Ui.ShowEmptyIfNeeded(rows, "restaurants"))
            return;

        var table = Ui.CreateTable("Revenue Function", "Restaurant", "Revenue");
        foreach (var row in rows)
            table.AddRow($"{row.RestaurantId} - {row.Name}", Ui.FormatMoney(row.Revenue));

        AnsiConsole.Write(table);
    }

    public async Task CreateAsync()
    {
        var restaurant = new Restaurant
        {
            Name = Ui.PromptRequired("Name:", 100),
            Address = Ui.PromptRequired("Address:", 100),
            PhoneNumber = Ui.PromptOptional("Phone number:", 20) ?? string.Empty,
            OpeningHours = Ui.PromptOptional("Opening hours:", 100) ?? string.Empty
        };

        await unitOfWork.Restaurants.AddAsync(restaurant);
        AnsiConsole.MarkupLine($"[green]Restaurant #{restaurant.RestaurantId} created.[/]");
    }
}

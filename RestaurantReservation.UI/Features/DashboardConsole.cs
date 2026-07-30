using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using Spectre.Console;

namespace RestaurantReservation.UI.Features;

public sealed class DashboardConsole(RestaurantReservationDbContext context, IUnitOfWork unitOfWork)
{
    public async Task ShowAsync()
    {
        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow("Restaurants", (await unitOfWork.Restaurants.GetAllAsync()).Count.ToString());
        grid.AddRow("Customers", (await unitOfWork.Customers.GetAllAsync()).Count.ToString());
        grid.AddRow("Tables", (await unitOfWork.Tables.GetAllAsync()).Count.ToString());
        grid.AddRow("Employees", (await unitOfWork.Employees.GetAllAsync()).Count.ToString());
        grid.AddRow("Menu items", (await unitOfWork.MenuItems.GetAllAsync()).Count.ToString());
        grid.AddRow("Reservations", (await context.Reservations.CountAsync()).ToString());
        grid.AddRow("Orders", (await unitOfWork.Orders.GetAllAsync()).Count.ToString());

        AnsiConsole.Write(new Panel(grid).Header("Current Data").Border(BoxBorder.Rounded));
    }
}

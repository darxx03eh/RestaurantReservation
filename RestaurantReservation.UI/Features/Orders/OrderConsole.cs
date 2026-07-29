using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.UI.Common;
using Spectre.Console;

namespace RestaurantReservation.UI.Features.Orders;

internal sealed class OrderConsole(
    RestaurantReservationDbContext context,
    IUnitOfWork unitOfWork,
    EntitySelector selector)
{
    public async Task ListWithMenuItemsAsync()
    {
        var reservationId = AnsiConsole.Ask<int>("Reservation id:");
        var orders = (await unitOfWork.Orders.ListOrdersAndMenuItemsAsync(reservationId)).ToList();
        if (Ui.ShowEmptyIfNeeded(orders, "orders for this reservation"))
            return;

        var table = Ui.CreateTable("Orders", "Order", "Date", "Total", "Items");

        foreach (var order in orders)
        {
            var items = order.OrderItems.Any()
                ? string.Join(", ", order.OrderItems.Select(item => $"{item.Item.Name} x{item.Quantity}"))
                : "No items";

            table.AddRow(order.OrderId.ToString(), Ui.FormatDate(order.OrderDate), Ui.FormatMoney(order.TotalAmount), items);
        }

        AnsiConsole.Write(table);
    }

    public async Task CreateAsync()
    {
        var reservation = await selector.SelectReservationAsync();
        if (reservation is null)
            return;

        var employees = await context.Employees
            .AsNoTracking()
            .Where(employee => employee.RestaurantId == reservation.RestaurantId)
            .OrderBy(employee => employee.EmployeeId)
            .ToListAsync();

        var menuItems = await context.MenuItems
            .AsNoTracking()
            .Where(item => item.RestaurantId == reservation.RestaurantId)
            .OrderBy(item => item.Name)
            .ToListAsync();

        if (employees.Count == 0 || menuItems.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]The reservation restaurant needs employees and menu items before an order can be created.[/]");
            return;
        }

        var employee = Ui.SelectEntity(employees, "Employee:", item => $"#{item.EmployeeId} - {Ui.FullName(item)} ({item.Position})");
        var selectedItems = AnsiConsole.Prompt(
            new MultiSelectionPrompt<MenuItem>()
                .Title("Order items:")
                .NotRequired()
                .UseConverter(item => $"#{item.ItemId} - {item.Name} ({Ui.FormatMoney(item.Price)})")
                .AddChoices(menuItems));

        if (selectedItems.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No order was created because no items were selected.[/]");
            return;
        }

        var quantities = new Dictionary<int, int>();
        foreach (var item in selectedItems)
        {
            quantities[item.ItemId] = AnsiConsole.Prompt(
                new TextPrompt<int>($"{item.Name} quantity:")
                    .Validate(value => value > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Quantity must be greater than zero.")));
        }

        var total = selectedItems.Sum(item => item.Price * quantities[item.ItemId]);
        var order = new Order
        {
            ReservationId = reservation.ReservationId,
            EmployeeId = employee.EmployeeId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = total
        };

        await unitOfWork.Orders.AddAsync(order);

        foreach (var item in selectedItems)
        {
            await unitOfWork.OrderItems.AddAsync(new OrderItem
            {
                OrderId = order.OrderId,
                ItemId = item.ItemId,
                Quantity = quantities[item.ItemId]
            });
        }

        AnsiConsole.MarkupLine($"[green]Order #{order.OrderId} created with total {Ui.FormatMoney(total)}.[/]");
    }
}

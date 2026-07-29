using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Entities.Views;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.UI.Common;
using Spectre.Console;

namespace RestaurantReservation.UI.Features.Reservations;

internal sealed class ReservationConsole(RestaurantReservationDbContext context, EntitySelector selector)
{
    public async Task ListAsync()
    {
        var reservations = await context.Reservations
            .AsNoTracking()
            .Include(reservation => reservation.Customer)
            .Include(reservation => reservation.Restaurant)
            .Include(reservation => reservation.Table)
            .OrderBy(reservation => reservation.ReservationDate)
            .ToListAsync();

        if (Ui.ShowEmptyIfNeeded(reservations, "reservations"))
            return;

        var table = Ui.CreateTable("Reservations", "Id", "Date", "Customer", "Restaurant", "Table", "Party");
        foreach (var reservation in reservations)
        {
            table.AddRow(
                reservation.ReservationId.ToString(),
                Ui.FormatDate(reservation.ReservationDate),
                Ui.FullName(reservation.Customer),
                reservation.Restaurant.Name,
                reservation.TableId.ToString(),
                reservation.PartySize.ToString());
        }

        AnsiConsole.Write(table);
    }

    public async Task ShowDetailsViewAsync()
    {
        var rows = await context.Set<ReservationDetailsView>()
            .AsNoTracking()
            .OrderBy(row => row.ReservationDate)
            .ToListAsync();

        if (Ui.ShowEmptyIfNeeded(rows, "reservation details"))
            return;

        var table = Ui.CreateTable("Reservation Details View", "Id", "Date", "Customer", "Phone", "Restaurant", "Party");
        foreach (var row in rows)
        {
            table.AddRow(
                row.ReservationId.ToString(),
                Ui.FormatDate(row.ReservationDate),
                row.CustomerName,
                row.CustomerPhoneNumber,
                row.RestaurantName,
                row.PartySize.ToString());
        }

        AnsiConsole.Write(table);
    }

    public async Task CreateAsync()
    {
        var customer = await selector.SelectCustomerAsync();
        var restaurant = await selector.SelectRestaurantAsync();
        if (customer is null || restaurant is null)
            return;

        var tables = await context.Tables
            .AsNoTracking()
            .Where(table => table.RestaurantId == restaurant.RestaurantId)
            .OrderBy(table => table.TableId)
            .ToListAsync();

        if (tables.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]This restaurant has no tables yet.[/]");
            return;
        }

        var selectedTable = Ui.SelectEntity(tables, "Table:", table => $"#{table.TableId} - capacity {table.Capacity}");
        var partySize = AnsiConsole.Prompt(
            new TextPrompt<int>("Party size:")
                .Validate(value => value > 0 && value <= selectedTable.Capacity
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"Party size must be between 1 and {selectedTable.Capacity}.")));

        var reservation = new Reservation
        {
            CustomerId = customer.CustomerId,
            RestaurantId = restaurant.RestaurantId,
            TableId = selectedTable.TableId,
            PartySize = partySize,
            ReservationDate = Ui.PromptDate("Reservation date (yyyy-MM-dd HH:mm):", DateTime.UtcNow.AddHours(1), mustBeFuture: true)
        };

        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
        AnsiConsole.MarkupLine($"[green]Reservation #{reservation.ReservationId} created.[/]");
    }
}

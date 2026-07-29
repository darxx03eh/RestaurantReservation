using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using Spectre.Console;

namespace RestaurantReservation.UI.Common;

internal sealed class EntitySelector(RestaurantReservationDbContext context, IUnitOfWork unitOfWork)
{
    public async Task<Customer?> SelectCustomerAsync()
    {
        var customers = (await unitOfWork.Customers.GetAllAsync())
            .OrderBy(customer => customer.CustomerId)
            .ToList();

        if (customers.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No customers found.[/]");
            AnsiConsole.MarkupLine("[yellow]No customers found.[/]");
            return null;
        }

        return Ui.SelectEntity(customers, "Customer:", customer => $"#{customer.CustomerId} - {Ui.FullName(customer)}");
    }

    public async Task<Restaurant?> SelectRestaurantAsync()
    {
        var restaurants = (await unitOfWork.Restaurants.GetAllAsync())
            .OrderBy(restaurant => restaurant.RestaurantId)
            .ToList();

        if (restaurants.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No restaurants found.[/]");
            return null;
        }

        return Ui.SelectEntity(restaurants, "Restaurant:", restaurant => $"#{restaurant.RestaurantId} - {restaurant.Name}");
    }

    public async Task<Reservation?> SelectReservationAsync()
    {
        var reservations = await context.Reservations
            .AsNoTracking()
            .Include(reservation => reservation.Customer)
            .Include(reservation => reservation.Restaurant)
            .OrderBy(reservation => reservation.ReservationId)
            .ToListAsync();

        if (reservations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No reservations found.[/]");
            return null;
        }

        return Ui.SelectEntity(
            reservations,
            "Reservation:",
            reservation => $"#{reservation.ReservationId} - {Ui.FullName(reservation.Customer)} at {reservation.Restaurant.Name} on {Ui.FormatDate(reservation.ReservationDate)}");
    }
}

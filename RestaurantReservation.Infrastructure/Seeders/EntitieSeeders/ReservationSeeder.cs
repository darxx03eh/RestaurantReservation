using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;

namespace RestaurantReservation.Infrastructure.Seeders.EntitieSeeders;

public class ReservationSeeder : ISeeder
{
    public async Task SeedAsync(RestaurantReservationDbContext context)
    {
        var numberOfReservations = await context.Reservations.CountAsync();
        if (numberOfReservations <= 0)
        {
            var reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    CustomerId = 1,
                    RestaurantId = 1,
                    TableId = 1,
                    ReservationDate = new DateTime(2026, 8, 1, 18, 0, 0),
                    PartySize = 2
                },
                new Reservation()
                {
                    CustomerId = 2,
                    RestaurantId = 2,
                    TableId = 2,
                    ReservationDate = new DateTime(2026, 8, 2, 19, 0, 0),
                    PartySize = 4
                },
                new Reservation()
                {
                    CustomerId = 3,
                    RestaurantId = 3,
                    TableId = 3,
                    ReservationDate = new DateTime(2026, 8, 3, 20, 0, 0),
                    PartySize = 5
                },
                new Reservation()
                {
                    CustomerId = 4,
                    RestaurantId = 4,
                    TableId = 4,
                    ReservationDate = new DateTime(2026, 8, 4, 18, 30, 0),
                    PartySize = 3
                },
                new Reservation()
                {
                    CustomerId = 5,
                    RestaurantId = 5,
                    TableId = 5,
                    ReservationDate = new DateTime(2026, 8, 5, 19, 30, 0),
                    PartySize = 4
                },
            };
            await context.BulkInsertAsync(reservations);
        }
    }
}
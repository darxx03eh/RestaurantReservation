using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class ReservationRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Reservation>(context), IReservationRepository
{
    private readonly RestaurantReservationDbContext _context = context;
    public async Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId)
    {
        var reservations = await _context.Reservations.AsNoTracking()
            .Include(r => r.Restaurant)
            .Where(r => r.CustomerId.Equals(customerId))
            .ToListAsync();

        return !reservations.Any()
            ? Enumerable.Empty<Reservation>()
            : reservations;
    }
}
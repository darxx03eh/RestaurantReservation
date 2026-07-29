using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class OrderRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Order>(context), IOrderRepository
{
    private readonly RestaurantReservationDbContext _context = context;
    public async Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
    {
        var orders = await _context.Orders.AsNoTracking()
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Item)
            .Where(o => o.ReservationId.Equals(reservationId))
            .ToListAsync();

        return !orders.Any()
            ? Enumerable.Empty<Order>()
            : orders;
    }
}
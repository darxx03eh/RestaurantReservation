using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class MenuItemRepository(RestaurantReservationDbContext context) 
    : GenericRepository<MenuItem>(context), IMenuItemRepository
{
    private readonly RestaurantReservationDbContext _context = context;
    public async Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
    {
        var items = await _context.Orders
            .AsNoTracking()
            .Where(o => o.ReservationId.Equals(reservationId))
            .SelectMany(o => o.OrderItems)
            .Select(oi => oi.Item)
            .Distinct()
            .ToListAsync();

        return !items.Any()
            ? Enumerable.Empty<MenuItem>()
            : items;
    }
}
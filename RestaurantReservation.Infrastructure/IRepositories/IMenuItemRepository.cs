using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.IRepositories.Generic;

namespace RestaurantReservation.Infrastructure.IRepositories;

public interface IMenuItemRepository : IGenericRepository<MenuItem>
{
    Task<IEnumerable<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}
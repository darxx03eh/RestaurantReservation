using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.IRepositories.Generic;

namespace RestaurantReservation.Infrastructure.IRepositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> ListOrdersAndMenuItemsAsync(int reservationId);
}
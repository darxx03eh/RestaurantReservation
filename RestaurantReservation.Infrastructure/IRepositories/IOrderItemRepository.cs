using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.IRepositories.Generic;

namespace RestaurantReservation.Infrastructure.IRepositories;

public interface IOrderItemRepository : IGenericRepository<OrderItem>
{
    
}
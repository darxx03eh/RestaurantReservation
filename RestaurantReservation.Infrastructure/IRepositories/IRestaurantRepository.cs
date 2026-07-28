using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.IRepositories.Generic;

namespace RestaurantReservation.Infrastructure.IRepositories;

public interface IRestaurantRepository : IGenericRepository<Restaurant>
{
    
}
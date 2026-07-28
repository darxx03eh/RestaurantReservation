using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class MenuItemRepository(RestaurantReservationDbContext context) 
    : GenericRepository<MenuItem>(context), IMenuItemRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class OrderRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Order>(context), IOrderRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
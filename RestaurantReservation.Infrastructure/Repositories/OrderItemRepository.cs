using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class OrderItemRepository(RestaurantReservationDbContext context) 
    : GenericRepository<OrderItem>(context), IOrderItemRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
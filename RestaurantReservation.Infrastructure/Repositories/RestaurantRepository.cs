using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class RestaurantRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Restaurant>(context), IRestaurantRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
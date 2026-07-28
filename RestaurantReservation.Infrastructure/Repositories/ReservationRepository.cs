using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class ReservationRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Reservation>(context), IReservationRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
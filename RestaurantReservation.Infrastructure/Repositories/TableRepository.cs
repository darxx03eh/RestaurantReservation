using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class TableRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Table>(context), ITableRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
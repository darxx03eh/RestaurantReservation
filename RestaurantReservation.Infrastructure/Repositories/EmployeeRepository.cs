using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class EmployeeRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
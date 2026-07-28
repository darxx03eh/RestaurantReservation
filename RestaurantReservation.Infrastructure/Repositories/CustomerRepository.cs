using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class CustomerRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Customer>(context), ICustomerRepository
{
    private readonly RestaurantReservationDbContext _context = context;
}
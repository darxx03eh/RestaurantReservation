using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories.Generic;

namespace RestaurantReservation.Infrastructure.Repositories;

public class EmployeeRepository(RestaurantReservationDbContext context) 
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    private readonly RestaurantReservationDbContext _context = context;
    public async Task<IEnumerable<Employee>> ListManagersAsync()
    {
        var managers = await _context.Employees.AsNoTracking()
            .Where(e => e.Position == Position.Manager)
            .ToListAsync();
        
        if (!managers.Any())
            return Enumerable.Empty<Employee>();
        return managers;
    }
}
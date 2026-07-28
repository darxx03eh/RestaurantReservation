using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.Infrastructure.IRepositories;
using RestaurantReservation.Infrastructure.Repositories;

namespace RestaurantReservation.Infrastructure.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly RestaurantReservationDbContext _context;
    public ICustomerRepository Customers { get; }
    public IReservationRepository Reservations { get; }
    public IOrderRepository Orders { get; }
    public IEmployeeRepository Employees { get; }
    public IMenuItemRepository MenuItems { get; }
    public IOrderItemRepository OrderItems { get; }
    public IRestaurantRepository Restaurants { get; }
    public ITableRepository Tables { get; }

    public UnitOfWork(RestaurantReservationDbContext context, CustomerRepository customers,
        ReservationRepository reservations, OrderRepository orders, EmployeeRepository employees,
        MenuItemRepository menuItems, OrderItemRepository orderItems, RestaurantRepository restaurants,
        TableRepository tables)
    {
        _context = context;
        Customers = customers;
        Reservations = reservations;
        Orders = orders;
        Employees = employees;
        MenuItems = menuItems;
        OrderItems = orderItems;
        Restaurants = restaurants;
        Tables = tables;
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync(cancellationToken);
}
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

    public UnitOfWork(RestaurantReservationDbContext context, ICustomerRepository customerRepository,
        IReservationRepository reservationRepository, IOrderRepository orderRepository, 
        IEmployeeRepository employeeRepository, IMenuItemRepository menuItemRepository, 
        IOrderItemRepository orderItemRepository, IRestaurantRepository restaurantRepository,
        ITableRepository tableRepository)
    {
        _context = context;
        Customers = customerRepository;
        Reservations = reservationRepository;
        Orders = orderRepository;
        Employees = employeeRepository;
        MenuItems = menuItemRepository;
        OrderItems = orderItemRepository;
        Restaurants = restaurantRepository;
        Tables = tableRepository;
        
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync(cancellationToken);
}
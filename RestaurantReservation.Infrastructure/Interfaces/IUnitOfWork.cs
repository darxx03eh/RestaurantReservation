using RestaurantReservation.Infrastructure.IRepositories;

namespace RestaurantReservation.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IReservationRepository Reservations { get; }
    IOrderRepository Orders { get; }
    IEmployeeRepository Employees { get; }
    IMenuItemRepository MenuItems { get; }
    IOrderItemRepository OrderItems { get; }
    IRestaurantRepository Restaurants { get; }
    ITableRepository Tables { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
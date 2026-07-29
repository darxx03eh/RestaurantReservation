using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.IRepositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(int customerId);
}
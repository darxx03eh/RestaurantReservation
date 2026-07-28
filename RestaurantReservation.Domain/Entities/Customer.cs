namespace RestaurantReservation.Domain.Entities;

public sealed class Customer
{
    public Customer() => Reservations = new HashSet<Reservation>();
    public int CustomerId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
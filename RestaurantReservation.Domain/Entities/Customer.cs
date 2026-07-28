namespace RestaurantReservation.Domain.Entities;

public sealed class Customer
{
    public Customer() => Reservations = new HashSet<Reservation>();
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
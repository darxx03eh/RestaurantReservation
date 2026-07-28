namespace RestaurantReservation.Domain.Entities;

public sealed class Reservation
{
    public Reservation() => Orders = new HashSet<Order>();
    public required int ReservationId { get; set; }
    public required DateTime ReservationDate { get; set; }
    public required int PartySize  { get; set; }
    
    public required int CustomerId { get; set; }
    public Customer Customer { get; set; }
    
    public required int TableId { get; set; }
    public Table Table { get; set; }
    
    public required int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    
    public ICollection<Order> Orders { get; set; }
}
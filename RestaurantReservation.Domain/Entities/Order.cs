namespace RestaurantReservation.Domain.Entities;

public sealed class Order
{
    public Order() => OrderItems = new HashSet<OrderItem>();
    public required int OrderId { get; set; }
    public required DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public required decimal TotalAmount { get; set; }
    
    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }
    
    public required int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public ICollection<OrderItem>  OrderItems { get; set; }
}
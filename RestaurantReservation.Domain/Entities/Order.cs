namespace RestaurantReservation.Domain.Entities;

public sealed class Order
{
    public Order() => OrderItems = new HashSet<OrderItem>();
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    
    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }
    
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public ICollection<OrderItem>  OrderItems { get; set; }
}
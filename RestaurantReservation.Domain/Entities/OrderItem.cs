namespace RestaurantReservation.Domain.Entities;

public sealed class OrderItem
{
    public required int OrderItemId { get; set; }
    public required int Quantity { get; set; }
    
    public required int OrderId { get; set; }
    public Order Order { get; set; }
    
    public required int ItemId { get; set; }
    public MenuItem Item { get; set; }
}
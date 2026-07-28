namespace RestaurantReservation.Domain.Entities;

public sealed class MenuItem
{
    public MenuItem() => OrderItems = new HashSet<OrderItem>();
    public required int ItemId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    
    public required int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
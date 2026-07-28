namespace RestaurantReservation.Domain.Entities;

public sealed class Table
{
    public Table() => Reservations = new HashSet<Reservation>();
    public required int TableId  { get; set; }
    public required int Capacity { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    
    public required int RestaurantId  { get; set; }
    public Restaurant Restaurant { get; set; }
}
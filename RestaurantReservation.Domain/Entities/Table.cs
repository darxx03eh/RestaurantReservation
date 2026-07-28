namespace RestaurantReservation.Domain.Entities;

public sealed class Table
{
    public Table() => Reservations = new HashSet<Reservation>();
    public int TableId  { get; set; }
    public int Capacity { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    
    public int RestaurantId  { get; set; }
    public Restaurant Restaurant { get; set; }
}
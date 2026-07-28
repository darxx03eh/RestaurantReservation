namespace RestaurantReservation.Domain.Entities;

public sealed class Restaurant
{
    public Restaurant()
    {
        Reservations = new  HashSet<Reservation>();
        Tables = new HashSet<Table>();
        MenuItems = new HashSet<MenuItem>();
        Employees = new HashSet<Employee>();
    }
    
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string OpeningHours  { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<Table> Tables { get; set; }
    public ICollection<Employee> Employees { get; set; }
    public ICollection<MenuItem> MenuItems { get; set; }
}
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities;

public sealed class Employee
{
    public Employee() => Orders = new HashSet<Order>();
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Position Position { get; set; }
    
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public ICollection<Order> Orders { get; set; }
}
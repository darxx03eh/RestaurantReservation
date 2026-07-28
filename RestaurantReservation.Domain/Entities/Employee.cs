using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities;

public sealed class Employee
{
    public Employee() => Orders = new HashSet<Order>();
    public required int EmployeeId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Position Position { get; set; }
    
    public required int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public ICollection<Order> Orders { get; set; }
}
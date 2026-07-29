namespace RestaurantReservation.Domain.Entities.Procedures;

public class CustomerReservationProcedure
{
    public int CustomerId { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
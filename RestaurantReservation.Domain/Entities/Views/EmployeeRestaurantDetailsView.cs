using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities.Views;

public class EmployeeRestaurantDetailsView
{
    public int EmployeeId  { get; set; }
    public string EmployeeName  { get; set; }
    public string Position  { get; set; }
    
    public int RestaurantId   { get; set; }
    public string RestauranName   { get; set; }
    public string RestauranAddress   { get; set; }
    public string RestauranPhoneNumber  { get; set; }
    public string OpeningHours   { get; set; }
}
namespace RestaurantReservation.Domain.Entities.Views;

public class ReservationDetailsView
{
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public int TableId { get; set; }
    
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhoneNumber { get; set; }
    
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantAddress { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities.Views;

namespace RestaurantReservation.Infrastructure.Configurations.ViewConfigurations;

public class ReservationDetailsViewConfigurations : IEntityTypeConfiguration<ReservationDetailsView>
{
    public void Configure(EntityTypeBuilder<ReservationDetailsView> builder)
    {
        builder.HasNoKey()
            .ToView("vw_ReservationsWithCustomerAndRestaurant");
    }
}
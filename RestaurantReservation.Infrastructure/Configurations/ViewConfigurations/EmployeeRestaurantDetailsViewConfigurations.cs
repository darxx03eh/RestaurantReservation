using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities.Views;

namespace RestaurantReservation.Infrastructure.Configurations.ViewConfigurations;

public class EmployeeRestaurantDetailsViewConfigurations : IEntityTypeConfiguration<EmployeeRestaurantDetailsView>
{
    public void Configure(EntityTypeBuilder<EmployeeRestaurantDetailsView> builder)
    {
        builder.HasNoKey()
            .ToView("vw_EmployeeRestaurantDetails");
    }
}
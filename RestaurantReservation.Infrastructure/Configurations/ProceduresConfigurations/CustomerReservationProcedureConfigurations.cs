using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities.Procedures;

namespace RestaurantReservation.Infrastructure.Configurations.ProceduresConfigurations;

public class CustomerReservationProcedureConfigurations : IEntityTypeConfiguration<CustomerReservationProcedure>
{
    public void Configure(EntityTypeBuilder<CustomerReservationProcedure> builder)
        => builder.HasNoKey();
}
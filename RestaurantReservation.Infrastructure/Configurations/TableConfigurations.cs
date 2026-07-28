using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class TableConfigurations : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable("Tables", t =>
        {
            t.HasCheckConstraint("CK_Tables_Capacity", $"[{nameof(Table.Capacity)}] > 0");

            t.HasCheckConstraint("CK_Tables_Capacity_Max", $"[{nameof(Table.Capacity)}] <= 20");
        });

        builder.HasKey(t => t.TableId);

        builder.Property(t => t.Capacity)
            .IsRequired();
        
        builder.Property(t => t.RestaurantId)
            .IsRequired();
        builder.HasOne(t => t.Restaurant)
            .WithMany(t => t.Tables)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Tables_Restaurants");
    }
}
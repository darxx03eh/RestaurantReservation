using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", o =>
        {
            o.HasCheckConstraint("CK_Orders_TotalAmount", $"[{nameof(Order.TotalAmount)}] >= 0");
            
            o.HasCheckConstraint("CK_Orders_Date", $"[{nameof(Order.OrderDate)}] <= GETUTCDATE()");
        });
        
        builder.HasKey(o => o.OrderId);
        
        builder.Property(o => o.OrderDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasPrecision(10, 2)
            .HasDefaultValue(0.0m);

        builder.Property(o => o.ReservationId)
            .IsRequired(false);
        builder.HasOne(o => o.Reservation)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.ReservationId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Orders_Reservations");

        builder.Property(o => o.EmployeeId)
            .IsRequired();
        builder.HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Orders_Employees");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", oi =>
        {
            oi.HasCheckConstraint("CK_OrderItems_Quantity",
                $"[{nameof(OrderItem.Quantity)}] > 0");
        });
        
        builder.HasKey(oi => oi.OrderItemId);

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.HasIndex(oi => new { oi.OrderId, oi.ItemId })
            .IsUnique()
            .HasDatabaseName("UX_OrderItems_Order_Item");
        
        builder.Property(oi => oi.OrderId)
            .IsRequired();
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItems_Orders");

        builder.Property(oi => oi.ItemId)
            .IsRequired();
        builder.HasOne(oi => oi.Item)
            .WithMany(i => i.OrderItems)
            .HasForeignKey(oi => oi.ItemId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_OrderItems_Items");
    }
}
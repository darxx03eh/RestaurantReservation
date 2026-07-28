using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class MenuItemConfigurations : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems", mi =>
        {
            mi.HasCheckConstraint("CK_MenuItems_Name", 
                $"LEN(LTRIM(RTRIM([{nameof(MenuItem.Name)}]))) > 0");

            mi.HasCheckConstraint("CK_MenuItems_Price", $"[{nameof(MenuItem.Price)}] > 0");
        });

        builder.HasKey(mi => mi.ItemId);

        builder.Property(mi => mi.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mi => mi.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(mi => mi.Price)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(mi => mi.RestaurantId)
            .IsRequired();
        builder.HasOne(mi => mi.Restaurant)
            .WithMany(r => r.MenuItems)
            .HasForeignKey(mi => mi.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("Fk_MenuItems_Restaurants");

        builder.HasIndex(mi => new { mi.RestaurantId, mi.Name })
            .IsUnique()
            .HasDatabaseName("UX_MenuItems_Restaurants_Name");
    }
}
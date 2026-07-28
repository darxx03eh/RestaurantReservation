using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class RestaurantConfigurations : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurants", r =>
        {
            r.HasCheckConstraint("CK_Restaurants_PhoneNumber",
                $"""
                 [{nameof(Restaurant.PhoneNumber)}] NOT LIKE '%[^0-9+]%'
                 AND LEN([{nameof(Restaurant.PhoneNumber)}]) BETWEEN 8 AND 16
                 AND (CHARINDEX('+', [{nameof(Restaurant.PhoneNumber)}]) = 0 OR CHARINDEX('+',  [{nameof(Restaurant.PhoneNumber)}]) = 1)
                 AND LEN([{nameof(Restaurant.PhoneNumber)}]) - LEN(REPLACE([{nameof(Restaurant.PhoneNumber)}], '+', '')) <= 1
                 AND SUBSTRING(REPLACE([{nameof(Restaurant.PhoneNumber)}], '+', ''), 1, 1) != '0'
                 """);
        });

        builder.HasKey(r => r.RestaurantId);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Address)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.OpeningHours)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(r => r.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(20);

        builder.HasIndex(r => r.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UX_Restaurants_PhoneNumber");
    }
}
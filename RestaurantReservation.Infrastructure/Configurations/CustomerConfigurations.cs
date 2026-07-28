using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", c =>
        {
            c.HasCheckConstraint(
                "CK_Customers_FirstName", 
                $"LEN(LTRIM(RTRIM([{nameof(Customer.FirstName)}]))) > 0");

            c.HasCheckConstraint("CK_Customers_LastName",
                $"LEN(LTRIM(RTRIM([{nameof(Customer.LastName)}]))) > 0");

            c.HasCheckConstraint("CK_Customers_Email",
                $"""
                 [{nameof(Customer.Email)}] LIKE '_%@_%.__%'
                 AND [{nameof(Customer.Email)}] NOT LIKE '%@%@%'
                 AND [{nameof(Customer.Email)}] NOT LIKE '% %'
                 """);

            c.HasCheckConstraint("CK_Customers_PhoneNumber",
                $"""
                 [{nameof(Customer.PhoneNumber)}] NOT LIKE '%[^0-9+]%'
                 AND LEN([{nameof(Customer.PhoneNumber)}]) BETWEEN 8 AND 16
                 AND (CHARINDEX('+', [{nameof(Customer.PhoneNumber)}]) = 0 OR CHARINDEX('+',  [{nameof(Customer.PhoneNumber)}]) = 1)
                 AND LEN([{nameof(Customer.PhoneNumber)}]) - LEN(REPLACE([{nameof(Customer.PhoneNumber)}], '+', '')) <= 1
                 AND SUBSTRING(REPLACE([{nameof(Customer.PhoneNumber)}], '+', ''), 1, 1) != '0'
                 """);

        });

        builder.HasKey(c => c.CustomerId);
        
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("UX_Customers_Email");

        builder.HasIndex(c => c.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UX_Customers_PhoneNumber");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Infrastructure.Configurations;

public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees", e =>
        {
            e.HasCheckConstraint(
                "CK_Customers_FirstName", 
                $"LEN(LTRIM(RTRIM([{nameof(Employee.FirstName)}]))) > 0");

            e.HasCheckConstraint("CK_Customers_LastName",
                $"LEN(LTRIM(RTRIM([{nameof(Employee.LastName)}]))) > 0");

            var positions = string.Join(", ", Enum.GetNames<Position>().Select(p => $"'{p}'"));
            e.HasCheckConstraint("CK_Employees_Position", $"[{nameof(Employee.Position)}] IN ({positions})");
        });

        builder.HasKey(e => e.EmployeeId);
        
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.Position)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RestaurantId)
            .IsRequired();
        builder.HasOne(e => e.Restaurant)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Employees_Restaurants");
    }
}
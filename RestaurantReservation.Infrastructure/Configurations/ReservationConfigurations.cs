using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Infrastructure.Configurations;

public class ReservationConfigurations : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations", r =>
        {
            r.HasCheckConstraint("CK_Reservations_PartySize", $"[{nameof(Reservation.PartySize)}] > 0");
            
            r.HasCheckConstraint("CK_Reservations_Date",  
                $"[{nameof(Reservation.ReservationDate)}] >= GETUTCDATE()");
        });
        
        builder.HasKey(r => r.ReservationId);

        builder.Property(r => r.ReservationDate)
            .IsRequired();

        builder.Property(r => r.PartySize)
            .IsRequired();
        
        builder.HasIndex(r => new { r.TableId, r.ReservationDate })
            .IsUnique()
            .HasDatabaseName("UX_Reservations_Table_Date");

        builder.Property(r => r.RestaurantId)
            .IsRequired();
        builder.HasOne(r => r.Customer)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Reservations_Customers");

        builder.Property(r => r.TableId)
            .IsRequired();
        builder.HasOne(r => r.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Reservations_Tables");
        
        builder.Property(r => r.RestaurantId)
            .IsRequired();
        builder.HasOne(r => r.Restaurant)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Reservations_Restaurants");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Infrastructure.EntityConfigurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(b => b.HallId)
            .IsRequired();
        
        builder.Property(b => b.CustomerName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(b => b.CustomerPhone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(b => b.Notes)
            .HasMaxLength(2000);
        
        builder.Property(b => b.EventDate)
            .IsRequired();
        
        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(b => b.PaymentStatus)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(b => b.AdvanceAmount)
            .HasPrecision(18, 2);
        
        builder.Property(b => b.ExternalPaymentReference)
            .HasMaxLength(200);
        
        builder.Property(b => b.CancelReason)
            .HasMaxLength(500);
        
        builder.Property(b => b.AdminComment)
            .HasMaxLength(1000);
        
        builder.Property(b => b.CreatedAtUtc)
            .IsRequired();
        
        builder.Property(b => b.UpdatedAtUtc)
            .IsRequired();
        
        // Relationship
        builder.HasOne(b => b.Hall)
            .WithMany()
            .HasForeignKey(b => b.HallId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes
        builder.HasIndex(b => b.HallId);
        builder.HasIndex(b => b.EventDate);
        builder.HasIndex(b => b.Status);
        
        // Unique constraint: (HallId, EventDate) cannot have >1 active booking
        // Active bookings are those with Status = Pending or Confirmed
        builder.HasIndex(b => new { b.HallId, b.EventDate })
            .HasFilter("[Status] IN (0, 1)") // Pending = 0, Confirmed = 1
            .IsUnique();
    }
}


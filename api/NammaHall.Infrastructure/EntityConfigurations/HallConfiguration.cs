using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Infrastructure.EntityConfigurations;

public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("Halls");
        
        builder.HasKey(h => h.Id);
        
        builder.Property(h => h.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(h => h.AddressLine)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(h => h.Village)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(h => h.Taluk)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(h => h.District)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(h => h.PostalCode)
            .HasMaxLength(10);
        
        builder.Property(h => h.Capacity)
            .IsRequired();
        
        builder.Property(h => h.PriceFrom)
            .HasPrecision(18, 2);
        
        builder.Property(h => h.PriceTo)
            .HasPrecision(18, 2);
        
        builder.Property(h => h.OwnerName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(h => h.OwnerPhone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(h => h.AlternatePhone)
            .HasMaxLength(20);
        
        builder.Property(h => h.GoogleMapsUrl)
            .HasMaxLength(1000);
        
        builder.Property(h => h.CreatedAtUtc)
            .IsRequired();
        
        builder.Property(h => h.UpdatedAtUtc)
            .IsRequired();
        
        // Relationships
        builder.HasMany(h => h.Images)
            .WithOne(hi => hi.Hall)
            .HasForeignKey(hi => hi.HallId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}


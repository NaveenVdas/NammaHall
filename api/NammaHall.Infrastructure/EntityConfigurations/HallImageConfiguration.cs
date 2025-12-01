using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Infrastructure.EntityConfigurations;

public class HallImageConfiguration : IEntityTypeConfiguration<HallImage>
{
    public void Configure(EntityTypeBuilder<HallImage> builder)
    {
        builder.ToTable("HallImages");
        
        builder.HasKey(hi => hi.Id);
        
        builder.Property(hi => hi.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(hi => hi.HallId)
            .IsRequired();
        
        builder.Property(hi => hi.ImageUrl)
            .IsRequired()
            .HasMaxLength(1000);
        
        builder.Property(hi => hi.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(hi => hi.CreatedAtUtc)
            .IsRequired();
        
        builder.Property(hi => hi.UpdatedAtUtc)
            .IsRequired();
        
        // Index for efficient queries
        builder.HasIndex(hi => hi.HallId);
        builder.HasIndex(hi => new { hi.HallId, hi.DisplayOrder });
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Infrastructure.EntityConfigurations;

public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
{
    public void Configure(EntityTypeBuilder<AdminUser> builder)
    {
        builder.ToTable("AdminUsers");
        
        builder.HasKey(au => au.Id);
        
        builder.Property(au => au.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(au => au.Email)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(au => au.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(au => au.DisplayName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(au => au.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(au => au.CreatedAtUtc)
            .IsRequired();
        
        builder.Property(au => au.UpdatedAtUtc)
            .IsRequired();
        
        // Unique index on Email
        builder.HasIndex(au => au.Email)
            .IsUnique();
    }
}


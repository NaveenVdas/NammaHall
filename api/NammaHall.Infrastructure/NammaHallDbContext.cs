using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NammaHall.Domain.DomainModel;
using NammaHall.Domain.DomainModel.HallAggregate;
using NammaHall.Infrastructure.EntityConfigurations;

namespace NammaHall.Infrastructure;

public class NammaHallDbContext : DbContext
{
    public NammaHallDbContext(DbContextOptions<NammaHallDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hall> Halls { get; set; }
    public DbSet<HallImage> HallImages { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NammaHallDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        // Suppress pending model changes warning during migrations
        // This is safe when applying migrations to a fresh database
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
}


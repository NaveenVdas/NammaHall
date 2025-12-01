using Microsoft.EntityFrameworkCore;
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
}


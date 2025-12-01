using Microsoft.EntityFrameworkCore;
using NammaHall.Application.Repository;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Infrastructure.Repository;

public sealed class BookingRepository : IBookingRepository
{
    private readonly NammaHallDbContext _db;

    public BookingRepository(NammaHallDbContext db)
    {
        _db = db;
    }

    public void Create(Booking booking) => _db.Bookings.Add(booking);

    public void Update(Booking booking) => _db.Bookings.Update(booking);

    public async Task<Booking?> GetById(int id, CancellationToken ct) =>
        await _db.Bookings
            .Include(b => b.Hall)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
}


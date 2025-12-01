using Microsoft.EntityFrameworkCore;
using NammaHall.Application.QueryServices;
using NammaHall.Domain.DomainModel;
using NammaHall.Domain.Enums;
using System.Collections.ObjectModel;

namespace NammaHall.Infrastructure.Queries;

public sealed class BookingQueries : IBookingQueries
{
    private readonly NammaHallDbContext _dbContext;

    public BookingQueries(NammaHallDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Booking?> GetById(int bookingId, CancellationToken ct) =>
        await _dbContext.Bookings
            .Include(b => b.Hall)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bookingId, ct);

    public async Task<ReadOnlyCollection<Booking>> GetBookings(BookingSearchParameters parameters, CancellationToken ct)
    {
        IQueryable<Booking> query = _dbContext.Bookings
            .Include(b => b.Hall)
            .AsNoTracking();

        if (parameters.HallId.HasValue)
        {
            query = query.Where(b => b.HallId == parameters.HallId.Value);
        }

        if (parameters.Status.HasValue)
        {
            query = query.Where(b => b.Status == parameters.Status.Value);
        }

        if (parameters.FromDate.HasValue)
        {
            query = query.Where(b => b.EventDate >= parameters.FromDate.Value);
        }

        if (parameters.ToDate.HasValue)
        {
            query = query.Where(b => b.EventDate <= parameters.ToDate.Value);
        }

        return (await query.OrderByDescending(b => b.CreatedAtUtc).ToListAsync(ct)).AsReadOnly();
    }
}


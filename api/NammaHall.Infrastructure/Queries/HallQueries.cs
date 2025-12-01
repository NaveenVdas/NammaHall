using Microsoft.EntityFrameworkCore;
using NammaHall.Application.QueryServices;
using NammaHall.Domain.DomainModel.HallAggregate;
using System.Collections.ObjectModel;

namespace NammaHall.Infrastructure.Queries;

public sealed class HallQueries : IHallQueries
{
    private readonly NammaHallDbContext _dbContext;

    public HallQueries(NammaHallDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReadOnlyCollection<Hall>> GetHalls(HallSearchParameters parameters, CancellationToken ct)
    {
        IQueryable<Hall> query = _dbContext.Halls
            .Include(h => h.Images)
            .AsNoTracking()
            .Where(h => h.IsPublished);

        if (!string.IsNullOrWhiteSpace(parameters.District))
        {
            query = query.Where(h => h.District.Contains(parameters.District));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Taluk))
        {
            query = query.Where(h => h.Taluk.Contains(parameters.Taluk));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Village))
        {
            query = query.Where(h => h.Village.Contains(parameters.Village));
        }

        if (parameters.MinGuests.HasValue)
        {
            query = query.Where(h => h.Capacity >= parameters.MinGuests.Value);
        }

        if (parameters.MaxGuests.HasValue)
        {
            query = query.Where(h => h.Capacity <= parameters.MaxGuests.Value);
        }

        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(h => h.PriceFrom.HasValue && h.PriceFrom >= parameters.MinPrice.Value);
        }

        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(h => h.PriceTo.HasValue && h.PriceTo <= parameters.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            string search = parameters.Search.ToLower();
            query = query.Where(h =>
                h.Name.ToLower().Contains(search) ||
                h.Village.ToLower().Contains(search) ||
                h.Taluk.ToLower().Contains(search) ||
                h.District.ToLower().Contains(search));
        }

        if (parameters.Date.HasValue)
        {
            // Check availability for the date
            var bookedHallIds = await _dbContext.Bookings
                .Where(b => b.EventDate == parameters.Date.Value &&
                           (b.Status == Domain.Enums.BookingStatus.Pending ||
                            b.Status == Domain.Enums.BookingStatus.Confirmed))
                .Select(b => b.HallId)
                .ToListAsync(ct);

            query = query.Where(h => !bookedHallIds.Contains(h.Id));
        }

        return (await query.ToListAsync(ct)).AsReadOnly();
    }

    public async Task<Hall?> GetHallById(int hallId, CancellationToken ct) =>
        await _dbContext.Halls
            .Include(h => h.Images.OrderBy(i => i.DisplayOrder))
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == hallId, ct);

    public async Task<bool> CheckAvailability(int hallId, DateOnly date, CancellationToken ct)
    {
        bool hasActiveBooking = await _dbContext.Bookings
            .AnyAsync(b => b.HallId == hallId &&
                          b.EventDate == date &&
                          (b.Status == Domain.Enums.BookingStatus.Pending ||
                           b.Status == Domain.Enums.BookingStatus.Confirmed), ct);

        return !hasActiveBooking;
    }
}


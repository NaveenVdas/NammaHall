using System.Collections.ObjectModel;
using NammaHall.Domain.DomainModel;
using NammaHall.Domain.Enums;

namespace NammaHall.Application.QueryServices;

public interface IBookingQueries
{
    Task<Booking?> GetById(int bookingId, CancellationToken ct);
    Task<ReadOnlyCollection<Booking>> GetBookings(BookingSearchParameters parameters, CancellationToken ct);
}

public sealed class BookingSearchParameters
{
    public int? HallId { get; init; }
    public BookingStatus? Status { get; init; }
    public DateOnly? FromDate { get; init; }
    public DateOnly? ToDate { get; init; }
}


using NammaHall.Application.QueryServices;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Application.Handlers.Booking;

public sealed class BookingGetHandler : IBookingGetHandler
{
    private readonly IBookingQueries _bookingQueries;

    public BookingGetHandler(IBookingQueries bookingQueries)
    {
        _bookingQueries = bookingQueries;
    }

    public async Task<Domain.DomainModel.Booking?> Handle(int bookingId, CancellationToken ct) =>
        await _bookingQueries.GetById(bookingId, ct);
}


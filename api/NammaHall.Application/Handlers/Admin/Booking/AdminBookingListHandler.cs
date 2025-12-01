using System.Collections.ObjectModel;
using NammaHall.Application.QueryServices;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Application.Handlers.Admin.Booking;

public sealed class AdminBookingListHandler : IAdminBookingListHandler
{
    private readonly IBookingQueries _bookingQueries;

    public AdminBookingListHandler(IBookingQueries bookingQueries)
    {
        _bookingQueries = bookingQueries;
    }

    public async Task<ReadOnlyCollection<Domain.DomainModel.Booking>> Handle(BookingSearchParameters parameters, CancellationToken ct) =>
        await _bookingQueries.GetBookings(parameters, ct);
}


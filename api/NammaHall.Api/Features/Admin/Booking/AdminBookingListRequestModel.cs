using NammaHall.Domain.Enums;

namespace NammaHall.Api.Features.Admin.Booking;

public sealed class AdminBookingListRequestModel
{
    public int? HallId { get; init; }
    public BookingStatus? Status { get; init; }
    public DateOnly? FromDate { get; init; }
    public DateOnly? ToDate { get; init; }

    public Application.QueryServices.BookingSearchParameters ToSearchParameters() =>
        new()
        {
            HallId = HallId,
            Status = Status,
            FromDate = FromDate,
            ToDate = ToDate
        };
}


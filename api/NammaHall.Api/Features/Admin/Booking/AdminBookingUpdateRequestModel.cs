using NammaHall.Domain.Enums;

namespace NammaHall.Api.Features.Admin.Booking;

public sealed class AdminBookingUpdateRequestModel
{
    public BookingStatus? Status { get; init; }
    public string? AdminComment { get; init; }
    public string? CancelReason { get; init; }
    public decimal? AdvanceAmount { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }

    public Application.Handlers.Admin.Booking.AdminBookingUpdateModel ToUpdateModel() =>
        new()
        {
            Status = Status,
            AdminComment = AdminComment,
            CancelReason = CancelReason,
            AdvanceAmount = AdvanceAmount,
            PaymentStatus = PaymentStatus
        };
}


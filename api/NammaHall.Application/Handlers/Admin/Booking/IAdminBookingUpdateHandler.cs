using NammaHall.Domain.Enums;

namespace NammaHall.Application.Handlers.Admin.Booking;

public interface IAdminBookingUpdateHandler
{
    Task<bool> Handle(int bookingId, AdminBookingUpdateModel model, CancellationToken ct);
}

public sealed class AdminBookingUpdateModel
{
    public BookingStatus? Status { get; init; }
    public string? AdminComment { get; init; }
    public string? CancelReason { get; init; }
    public decimal? AdvanceAmount { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }
}


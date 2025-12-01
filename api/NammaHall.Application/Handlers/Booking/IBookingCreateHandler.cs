using NammaHall.Application.Handlers.Booking;

namespace NammaHall.Application.Handlers.Booking;

public interface IBookingCreateHandler
{
    Task<BookingCreateResult> Handle(BookingCreateModel model, CancellationToken ct);
}

public sealed class BookingCreateModel
{
    public int HallId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public DateOnly EventDate { get; init; }
    public int? GuestCount { get; init; }
    public string? Notes { get; init; }
}

public sealed class BookingCreateResult
{
    public int BookingId { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsConflicted { get; init; }
    public bool IsInvalidRequest { get; init; }

    public static BookingCreateResult Success(int bookingId) =>
        new() { BookingId = bookingId, IsSuccess = true };

    public static BookingCreateResult Conflicted() =>
        new() { IsConflicted = true };

    public static BookingCreateResult InvalidRequest() =>
        new() { IsInvalidRequest = true };
}


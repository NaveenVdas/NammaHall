namespace NammaHall.Api.Features.Booking;

public sealed class BookingCreateRequestModel
{
    public required int HallId { get; init; }
    public required string CustomerName { get; init; }
    public required string CustomerPhone { get; init; }
    public required DateOnly EventDate { get; init; }
    public int? GuestCount { get; init; }
    public string? Notes { get; init; }

    public Application.Handlers.Booking.BookingCreateModel ToCreateModel() =>
        new()
        {
            HallId = HallId,
            CustomerName = CustomerName,
            CustomerPhone = CustomerPhone,
            EventDate = EventDate,
            GuestCount = GuestCount,
            Notes = Notes
        };
}


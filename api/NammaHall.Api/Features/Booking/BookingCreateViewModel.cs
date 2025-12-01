namespace NammaHall.Api.Features.Booking;

public sealed class BookingCreateViewModel
{
    public int BookingId { get; init; }
    public string Summary { get; init; } = string.Empty;

    public BookingCreateViewModel(Application.Handlers.Booking.BookingCreateResult result)
    {
        BookingId = result.BookingId;
        Summary = $"Booking {result.BookingId} created successfully";
    }
}


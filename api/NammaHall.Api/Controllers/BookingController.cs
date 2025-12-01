using Microsoft.AspNetCore.Mvc;
using NammaHall.Api.Features.Booking;
using NammaHall.Application.Handlers.Booking;

namespace NammaHall.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public sealed class BookingController : ControllerBase
{
    private readonly IBookingCreateHandler _bookingCreateHandler;
    private readonly IBookingGetHandler _bookingGetHandler;

    public BookingController(
        IBookingCreateHandler bookingCreateHandler,
        IBookingGetHandler bookingGetHandler)
    {
        _bookingCreateHandler = bookingCreateHandler;
        _bookingGetHandler = bookingGetHandler;
    }

    [HttpPost]
    [ProducesResponseType<BookingCreateViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequestModel request, CancellationToken ct)
    {
        var result = await _bookingCreateHandler.Handle(request.ToCreateModel(), ct);

        return result switch
        {
            { IsSuccess: true } => Ok(new BookingCreateViewModel(result)),
            { IsConflicted: true } => Conflict(),
            { IsInvalidRequest: true } => BadRequest(),
            _ => throw new InvalidOperationException("Unhandled booking creation result.")
        };
    }

    [HttpGet("{bookingId:int}")]
    [ProducesResponseType<BookingViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooking([FromRoute] int bookingId, CancellationToken ct)
    {
        var booking = await _bookingGetHandler.Handle(bookingId, ct);
        return booking is null
            ? NotFound()
            : Ok(new BookingViewModel(booking));
    }
}


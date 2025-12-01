using Microsoft.AspNetCore.Mvc;
using NammaHall.Api.Features.Admin.Booking;
using NammaHall.Api.Features.Booking;
using NammaHall.Application.Handlers.Admin.Booking;
using NammaHall.Application.Handlers.Booking;
using System.Collections.ObjectModel;

namespace NammaHall.Api.Controllers;

[ApiController]
[Route("api/admin/bookings")]
public sealed class AdminBookingController : ControllerBase
{
    private readonly IAdminBookingListHandler _adminBookingListHandler;
    private readonly IBookingGetHandler _bookingGetHandler;
    private readonly IAdminBookingUpdateHandler _adminBookingUpdateHandler;

    public AdminBookingController(
        IAdminBookingListHandler adminBookingListHandler,
        IBookingGetHandler bookingGetHandler,
        IAdminBookingUpdateHandler adminBookingUpdateHandler)
    {
        _adminBookingListHandler = adminBookingListHandler;
        _bookingGetHandler = bookingGetHandler;
        _adminBookingUpdateHandler = adminBookingUpdateHandler;
    }

    [HttpGet]
    [ProducesResponseType<ReadOnlyCollection<BookingViewModel>>(StatusCodes.Status200OK)]
    public async Task<ReadOnlyCollection<BookingViewModel>> GetBookings([FromQuery] AdminBookingListRequestModel request, CancellationToken ct)
    {
        var bookings = await _adminBookingListHandler.Handle(request.ToSearchParameters(), ct);
        return bookings.Select(b => new BookingViewModel(b)).ToList().AsReadOnly();
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

    [HttpPatch("{bookingId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBooking([FromRoute] int bookingId, [FromBody] AdminBookingUpdateRequestModel request, CancellationToken ct)
    {
        var success = await _adminBookingUpdateHandler.Handle(bookingId, request.ToUpdateModel(), ct);
        return success ? Ok() : NotFound();
    }
}


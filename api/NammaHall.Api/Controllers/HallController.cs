using Microsoft.AspNetCore.Mvc;
using NammaHall.Api.Features.Hall;
using NammaHall.Application.Handlers.Hall;

namespace NammaHall.Api.Controllers;

[ApiController]
[Route("api/halls")]
public sealed class HallController : ControllerBase
{
    private readonly IHallListHandler _hallListHandler;
    private readonly IHallGetHandler _hallGetHandler;
    private readonly IHallAvailabilityHandler _hallAvailabilityHandler;

    public HallController(
        IHallListHandler hallListHandler,
        IHallGetHandler hallGetHandler,
        IHallAvailabilityHandler hallAvailabilityHandler)
    {
        _hallListHandler = hallListHandler;
        _hallGetHandler = hallGetHandler;
        _hallAvailabilityHandler = hallAvailabilityHandler;
    }

    [HttpGet]
    [ProducesResponseType<HallListViewModel>(StatusCodes.Status200OK)]
    public async Task<HallListViewModel> GetHalls([FromQuery] HallListRequestModel request, CancellationToken ct)
    {
        var halls = await _hallListHandler.Handle(request.ToSearchParameters(), ct);
        return new HallListViewModel(halls);
    }

    [HttpGet("{hallId:int}")]
    [ProducesResponseType<HallViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHall([FromRoute] int hallId, CancellationToken ct)
    {
        var hall = await _hallGetHandler.Handle(hallId, ct);
        return hall is null
            ? NotFound()
            : Ok(new HallViewModel(hall));
    }

    [HttpGet("{hallId:int}/availability")]
    [ProducesResponseType<HallAvailabilityViewModel>(StatusCodes.Status200OK)]
    public async Task<HallAvailabilityViewModel> CheckAvailability(
        [FromRoute] int hallId,
        [FromQuery] HallAvailabilityRequestModel request,
        CancellationToken ct)
    {
        var isAvailable = await _hallAvailabilityHandler.Handle(
            new HallAvailabilityRequest { HallId = hallId, Date = request.Date }, ct);

        return new HallAvailabilityViewModel
        {
            HallId = hallId,
            Date = request.Date,
            IsAvailable = isAvailable
        };
    }
}


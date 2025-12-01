using Microsoft.AspNetCore.Mvc;
using NammaHall.Api.Features.Admin.Hall;
using NammaHall.Api.Features.Hall;
using NammaHall.Application.Handlers.Admin.Hall;
using NammaHall.Application.Handlers.Hall;

namespace NammaHall.Api.Controllers;

[ApiController]
[Route("api/admin/halls")]
public sealed class AdminHallController : ControllerBase
{
    private readonly IAdminHallListHandler _adminHallListHandler;
    private readonly IHallGetHandler _hallGetHandler;
    private readonly IAdminHallCreateHandler _adminHallCreateHandler;
    private readonly IAdminHallUpdateHandler _adminHallUpdateHandler;
    private readonly IAdminHallDeleteHandler _adminHallDeleteHandler;

    public AdminHallController(
        IAdminHallListHandler adminHallListHandler,
        IHallGetHandler hallGetHandler,
        IAdminHallCreateHandler adminHallCreateHandler,
        IAdminHallUpdateHandler adminHallUpdateHandler,
        IAdminHallDeleteHandler adminHallDeleteHandler)
    {
        _adminHallListHandler = adminHallListHandler;
        _hallGetHandler = hallGetHandler;
        _adminHallCreateHandler = adminHallCreateHandler;
        _adminHallUpdateHandler = adminHallUpdateHandler;
        _adminHallDeleteHandler = adminHallDeleteHandler;
    }

    [HttpGet]
    [ProducesResponseType<HallListViewModel>(StatusCodes.Status200OK)]
    public async Task<HallListViewModel> GetHalls([FromQuery] AdminHallListRequestModel request, CancellationToken ct)
    {
        var halls = await _adminHallListHandler.Handle(request.ToSearchParameters(), ct);
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

    [HttpPost]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<int> CreateHall([FromBody] AdminHallCreateRequestModel request, CancellationToken ct)
    {
        return await _adminHallCreateHandler.Handle(request.ToCreateModel(), ct);
    }

    [HttpPut("{hallId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHall([FromRoute] int hallId, [FromBody] AdminHallUpdateRequestModel request, CancellationToken ct)
    {
        var success = await _adminHallUpdateHandler.Handle(hallId, request.ToUpdateModel(), ct);
        return success ? Ok() : NotFound();
    }

    [HttpDelete("{hallId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHall([FromRoute] int hallId, CancellationToken ct)
    {
        var success = await _adminHallDeleteHandler.Handle(hallId, ct);
        return success ? Ok() : NotFound();
    }

    [HttpPost("{hallId:int}/publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishHall([FromRoute] int hallId, CancellationToken ct)
    {
        var hall = await _hallGetHandler.Handle(hallId, ct);
        if (hall == null) return NotFound();

        var updateModel = new Application.Handlers.Admin.Hall.AdminHallUpdateModel
        {
            Name = hall.Name,
            AddressLine = hall.AddressLine,
            Village = hall.Village,
            Taluk = hall.Taluk,
            District = hall.District,
            PostalCode = hall.PostalCode,
            Capacity = hall.Capacity,
            PriceFrom = hall.PriceFrom,
            PriceTo = hall.PriceTo,
            IsAc = hall.IsAc,
            HasDiningHall = hall.HasDiningHall,
            HasParking = hall.HasParking,
            HasRooms = hall.HasRooms,
            OwnerName = hall.OwnerName,
            OwnerPhone = hall.OwnerPhone,
            AlternatePhone = hall.AlternatePhone,
            GoogleMapsUrl = hall.GoogleMapsUrl,
            IsPublished = true,
            IsVerified = hall.IsVerified,
            ImageUrls = hall.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList()
        };

        var success = await _adminHallUpdateHandler.Handle(hallId, updateModel, ct);
        return success ? Ok() : NotFound();
    }

    [HttpPost("{hallId:int}/unpublish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnpublishHall([FromRoute] int hallId, CancellationToken ct)
    {
        var hall = await _hallGetHandler.Handle(hallId, ct);
        if (hall == null) return NotFound();

        var updateModel = new Application.Handlers.Admin.Hall.AdminHallUpdateModel
        {
            Name = hall.Name,
            AddressLine = hall.AddressLine,
            Village = hall.Village,
            Taluk = hall.Taluk,
            District = hall.District,
            PostalCode = hall.PostalCode,
            Capacity = hall.Capacity,
            PriceFrom = hall.PriceFrom,
            PriceTo = hall.PriceTo,
            IsAc = hall.IsAc,
            HasDiningHall = hall.HasDiningHall,
            HasParking = hall.HasParking,
            HasRooms = hall.HasRooms,
            OwnerName = hall.OwnerName,
            OwnerPhone = hall.OwnerPhone,
            AlternatePhone = hall.AlternatePhone,
            GoogleMapsUrl = hall.GoogleMapsUrl,
            IsPublished = false,
            IsVerified = hall.IsVerified,
            ImageUrls = hall.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList()
        };

        var success = await _adminHallUpdateHandler.Handle(hallId, updateModel, ct);
        return success ? Ok() : NotFound();
    }

    [HttpPost("{hallId:int}/verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyHall([FromRoute] int hallId, CancellationToken ct)
    {
        var hall = await _hallGetHandler.Handle(hallId, ct);
        if (hall == null) return NotFound();

        var updateModel = new Application.Handlers.Admin.Hall.AdminHallUpdateModel
        {
            Name = hall.Name,
            AddressLine = hall.AddressLine,
            Village = hall.Village,
            Taluk = hall.Taluk,
            District = hall.District,
            PostalCode = hall.PostalCode,
            Capacity = hall.Capacity,
            PriceFrom = hall.PriceFrom,
            PriceTo = hall.PriceTo,
            IsAc = hall.IsAc,
            HasDiningHall = hall.HasDiningHall,
            HasParking = hall.HasParking,
            HasRooms = hall.HasRooms,
            OwnerName = hall.OwnerName,
            OwnerPhone = hall.OwnerPhone,
            AlternatePhone = hall.AlternatePhone,
            GoogleMapsUrl = hall.GoogleMapsUrl,
            IsPublished = hall.IsPublished,
            IsVerified = true,
            ImageUrls = hall.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList()
        };

        var success = await _adminHallUpdateHandler.Handle(hallId, updateModel, ct);
        return success ? Ok() : NotFound();
    }
}


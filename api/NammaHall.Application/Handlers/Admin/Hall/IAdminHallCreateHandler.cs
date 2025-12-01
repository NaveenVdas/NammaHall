using NammaHall.Application.Handlers.Admin.Hall;

namespace NammaHall.Application.Handlers.Admin.Hall;

public interface IAdminHallCreateHandler
{
    Task<int> Handle(AdminHallCreateModel model, CancellationToken ct);
}

public sealed class AdminHallCreateModel
{
    public string Name { get; init; } = string.Empty;
    public string AddressLine { get; init; } = string.Empty;
    public string Village { get; init; } = string.Empty;
    public string Taluk { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public string? PostalCode { get; init; }
    public int Capacity { get; init; }
    public decimal? PriceFrom { get; init; }
    public decimal? PriceTo { get; init; }
    public bool IsAc { get; init; }
    public bool HasDiningHall { get; init; }
    public bool HasParking { get; init; }
    public bool HasRooms { get; init; }
    public string OwnerName { get; init; } = string.Empty;
    public string OwnerPhone { get; init; } = string.Empty;
    public string? AlternatePhone { get; init; }
    public string? GoogleMapsUrl { get; init; }
    public bool IsPublished { get; init; }
    public bool IsVerified { get; init; }
    public List<string> ImageUrls { get; init; } = new();
}


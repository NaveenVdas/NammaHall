namespace NammaHall.Api.Features.Admin.Hall;

public sealed class AdminHallUpdateRequestModel
{
    public required string Name { get; init; }
    public required string AddressLine { get; init; }
    public required string Village { get; init; }
    public required string Taluk { get; init; }
    public required string District { get; init; }
    public string? PostalCode { get; init; }
    public required int Capacity { get; init; }
    public decimal? PriceFrom { get; init; }
    public decimal? PriceTo { get; init; }
    public bool IsAc { get; init; }
    public bool HasDiningHall { get; init; }
    public bool HasParking { get; init; }
    public bool HasRooms { get; init; }
    public required string OwnerName { get; init; }
    public required string OwnerPhone { get; init; }
    public string? AlternatePhone { get; init; }
    public string? GoogleMapsUrl { get; init; }
    public bool IsPublished { get; init; }
    public bool IsVerified { get; init; }
    public List<string> ImageUrls { get; init; } = new();

    public Application.Handlers.Admin.Hall.AdminHallUpdateModel ToUpdateModel() =>
        new()
        {
            Name = Name,
            AddressLine = AddressLine,
            Village = Village,
            Taluk = Taluk,
            District = District,
            PostalCode = PostalCode,
            Capacity = Capacity,
            PriceFrom = PriceFrom,
            PriceTo = PriceTo,
            IsAc = IsAc,
            HasDiningHall = HasDiningHall,
            HasParking = HasParking,
            HasRooms = HasRooms,
            OwnerName = OwnerName,
            OwnerPhone = OwnerPhone,
            AlternatePhone = AlternatePhone,
            GoogleMapsUrl = GoogleMapsUrl,
            IsPublished = IsPublished,
            IsVerified = IsVerified,
            ImageUrls = ImageUrls
        };
}


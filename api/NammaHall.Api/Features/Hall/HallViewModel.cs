namespace NammaHall.Api.Features.Hall;

public sealed class HallViewModel
{
    public int Id { get; init; }
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
    public string? GoogleMapsUrl { get; init; }
    public List<string> Images { get; init; } = new();

    public HallViewModel(Domain.DomainModel.HallAggregate.Hall hall)
    {
        Id = hall.Id;
        Name = hall.Name;
        AddressLine = hall.AddressLine;
        Village = hall.Village;
        Taluk = hall.Taluk;
        District = hall.District;
        PostalCode = hall.PostalCode;
        Capacity = hall.Capacity;
        PriceFrom = hall.PriceFrom;
        PriceTo = hall.PriceTo;
        IsAc = hall.IsAc;
        HasDiningHall = hall.HasDiningHall;
        HasParking = hall.HasParking;
        HasRooms = hall.HasRooms;
        OwnerName = hall.OwnerName;
        OwnerPhone = hall.OwnerPhone;
        GoogleMapsUrl = hall.GoogleMapsUrl;
        Images = hall.Images.OrderBy(i => i.DisplayOrder).Select(i => i.ImageUrl).ToList();
    }
}


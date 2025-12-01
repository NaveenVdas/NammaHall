namespace NammaHall.Domain.DomainModel.HallAggregate;

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string Village { get; set; } = string.Empty;
    public string Taluk { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string? PostalCode { get; set; }
    public int Capacity { get; set; }
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    
    // Facilities
    public bool IsAc { get; set; }
    public bool HasDiningHall { get; set; }
    public bool HasParking { get; set; }
    public bool HasRooms { get; set; }
    
    // Owner Info
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerPhone { get; set; } = string.Empty;
    public string? AlternatePhone { get; set; }
    
    // Other Info
    public string? GoogleMapsUrl { get; set; }
    public bool IsPublished { get; set; }
    public bool IsVerified { get; set; }
    
    // Audit
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    
    // Navigation Properties
    public ICollection<HallImage> Images { get; set; } = new List<HallImage>();
}


namespace NammaHall.Domain.DomainModel.HallAggregate;

public class HallImage
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    
    // Navigation Property
    public Hall Hall { get; set; } = null!;
}


using NammaHall.Domain.DomainModel.HallAggregate;
using NammaHall.Domain.Enums;

namespace NammaHall.Domain.DomainModel;

public class Booking
{
    public int Id { get; set; }
    public int HallId { get; set; }
    
    // User Information
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int? GuestCount { get; set; }
    public string? Notes { get; set; }
    
    // Event Info
    public DateOnly EventDate { get; set; }
    
    // Booking Status
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    
    // Payment Info
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.None;
    public decimal? AdvanceAmount { get; set; }
    public string? ExternalPaymentReference { get; set; }
    
    // Admin Handling
    public DateTime? ConfirmedAtUtc { get; set; }
    public DateTime? CancelledAtUtc { get; set; }
    public string? CancelReason { get; set; }
    public string? AdminComment { get; set; }
    
    // Audit
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    
    // Navigation Property
    public Hall Hall { get; set; } = null!;
}

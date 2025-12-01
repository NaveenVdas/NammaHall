using NammaHall.Domain.DomainModel;
using NammaHall.Domain.Enums;

namespace NammaHall.Api.Features.Booking;

public sealed class BookingViewModel
{
    public int Id { get; init; }
    public int HallId { get; init; }
    public string HallName { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public int? GuestCount { get; init; }
    public string? Notes { get; init; }
    public DateOnly EventDate { get; init; }
    public BookingStatus Status { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public decimal? AdvanceAmount { get; init; }
    public DateTime CreatedAtUtc { get; init; }

    public BookingViewModel(Domain.DomainModel.Booking booking)
    {
        Id = booking.Id;
        HallId = booking.HallId;
        HallName = booking.Hall.Name;
        CustomerName = booking.CustomerName;
        CustomerPhone = booking.CustomerPhone;
        GuestCount = booking.GuestCount;
        Notes = booking.Notes;
        EventDate = booking.EventDate;
        Status = booking.Status;
        PaymentStatus = booking.PaymentStatus;
        AdvanceAmount = booking.AdvanceAmount;
        CreatedAtUtc = booking.CreatedAtUtc;
    }
}


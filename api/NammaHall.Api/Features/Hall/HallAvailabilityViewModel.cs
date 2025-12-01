namespace NammaHall.Api.Features.Hall;

public sealed class HallAvailabilityViewModel
{
    public int HallId { get; init; }
    public DateOnly Date { get; init; }
    public bool IsAvailable { get; init; }
}


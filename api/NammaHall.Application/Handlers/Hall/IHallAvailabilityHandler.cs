using NammaHall.Application.Common.Handlers;

namespace NammaHall.Application.Handlers.Hall;

public interface IHallAvailabilityHandler : IQueryHandler<HallAvailabilityRequest, bool>
{
}

public sealed class HallAvailabilityRequest
{
    public int HallId { get; init; }
    public DateOnly Date { get; init; }
}


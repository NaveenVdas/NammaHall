using NammaHall.Application.QueryServices;

namespace NammaHall.Application.Handlers.Hall;

public sealed class HallAvailabilityHandler : IHallAvailabilityHandler
{
    private readonly IHallQueries _hallQueries;

    public HallAvailabilityHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<bool> Handle(HallAvailabilityRequest request, CancellationToken ct) =>
        await _hallQueries.CheckAvailability(request.HallId, request.Date, ct);
}


using NammaHall.Application.QueryServices;

namespace NammaHall.Application.Handlers.Hall;

public sealed class HallGetHandler : IHallGetHandler
{
    private readonly IHallQueries _hallQueries;

    public HallGetHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<Domain.DomainModel.HallAggregate.Hall?> Handle(int hallId, CancellationToken ct) =>
        await _hallQueries.GetHallById(hallId, ct);
}


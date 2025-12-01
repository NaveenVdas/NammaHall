using System.Collections.ObjectModel;
using NammaHall.Application.QueryServices;

namespace NammaHall.Application.Handlers.Hall;

public sealed class HallListHandler : IHallListHandler
{
    private readonly IHallQueries _hallQueries;

    public HallListHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<ReadOnlyCollection<Domain.DomainModel.HallAggregate.Hall>> Handle(HallSearchParameters parameters, CancellationToken ct) =>
        await _hallQueries.GetHalls(parameters, ct);
}


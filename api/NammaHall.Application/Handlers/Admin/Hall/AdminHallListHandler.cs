using System.Collections.ObjectModel;
using NammaHall.Application.QueryServices;

namespace NammaHall.Application.Handlers.Admin.Hall;

public sealed class AdminHallListHandler : IAdminHallListHandler
{
    private readonly IHallQueries _hallQueries;

    public AdminHallListHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<ReadOnlyCollection<Domain.DomainModel.HallAggregate.Hall>> Handle(AdminHallSearchParameters parameters, CancellationToken ct)
    {
        var searchParams = new HallSearchParameters
        {
            District = parameters.District,
            Taluk = parameters.Taluk,
            Village = parameters.Village,
            Search = parameters.Search
        };

        var halls = await _hallQueries.GetHalls(searchParams, ct);

        // Filter by IsPublished if specified
        if (parameters.IsPublished.HasValue)
        {
            halls = halls.Where(h => h.IsPublished == parameters.IsPublished.Value).ToList().AsReadOnly();
        }

        return halls;
    }
}


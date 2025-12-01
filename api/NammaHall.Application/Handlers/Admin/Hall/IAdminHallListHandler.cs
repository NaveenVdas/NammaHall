using System.Collections.ObjectModel;
using NammaHall.Application.Common.Handlers;

namespace NammaHall.Application.Handlers.Admin.Hall;

public interface IAdminHallListHandler : IQueryHandler<AdminHallSearchParameters, ReadOnlyCollection<Domain.DomainModel.HallAggregate.Hall>>
{
}

public sealed class AdminHallSearchParameters
{
    public string? Search { get; init; }
    public string? District { get; init; }
    public string? Taluk { get; init; }
    public string? Village { get; init; }
    public bool? IsPublished { get; init; }
}


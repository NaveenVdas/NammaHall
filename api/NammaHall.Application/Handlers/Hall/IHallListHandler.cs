using System.Collections.ObjectModel;
using NammaHall.Application.Common.Handlers;
using NammaHall.Application.QueryServices;

namespace NammaHall.Application.Handlers.Hall;

public interface IHallListHandler : IQueryHandler<HallSearchParameters, ReadOnlyCollection<Domain.DomainModel.HallAggregate.Hall>>
{
}


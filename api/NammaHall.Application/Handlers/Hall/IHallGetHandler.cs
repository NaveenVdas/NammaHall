using NammaHall.Application.Common.Handlers;

namespace NammaHall.Application.Handlers.Hall;

public interface IHallGetHandler : IQueryHandler<int, Domain.DomainModel.HallAggregate.Hall?>
{
}


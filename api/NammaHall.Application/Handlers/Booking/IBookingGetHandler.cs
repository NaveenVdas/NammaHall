using NammaHall.Application.Common.Handlers;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Application.Handlers.Booking;

public interface IBookingGetHandler : IQueryHandler<int, Domain.DomainModel.Booking?>
{
}


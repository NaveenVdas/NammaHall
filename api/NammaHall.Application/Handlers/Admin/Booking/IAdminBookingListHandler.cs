using System.Collections.ObjectModel;
using NammaHall.Application.Common.Handlers;
using NammaHall.Application.QueryServices;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Application.Handlers.Admin.Booking;

public interface IAdminBookingListHandler : IQueryHandler<BookingSearchParameters, ReadOnlyCollection<Domain.DomainModel.Booking>>
{
}


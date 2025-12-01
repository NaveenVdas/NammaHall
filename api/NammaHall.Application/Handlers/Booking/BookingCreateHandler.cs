using NammaHall.Application.QueryServices;
using NammaHall.Application.Repository;
using NammaHall.Application.UnitOfWork;
using NammaHall.Domain.DomainModel;
using NammaHall.Domain.Enums;

namespace NammaHall.Application.Handlers.Booking;

public sealed class BookingCreateHandler : IBookingCreateHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHallQueries _hallQueries;
    private readonly IUnitOfWork _unitOfWork;

    public BookingCreateHandler(
        IBookingRepository bookingRepository,
        IHallQueries hallQueries,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _hallQueries = hallQueries;
        _unitOfWork = unitOfWork;
    }

    public async Task<BookingCreateResult> Handle(BookingCreateModel model, CancellationToken ct)
    {
        // Check if hall exists
        var hall = await _hallQueries.GetHallById(model.HallId, ct);
        if (hall == null)
        {
            return BookingCreateResult.InvalidRequest();
        }

        // Check availability
        bool isAvailable = await _hallQueries.CheckAvailability(model.HallId, model.EventDate, ct);
        if (!isAvailable)
        {
            return BookingCreateResult.Conflicted();
        }

        // Create booking
        var booking = new Domain.DomainModel.Booking
        {
            HallId = model.HallId,
            CustomerName = model.CustomerName,
            CustomerPhone = model.CustomerPhone,
            EventDate = model.EventDate,
            GuestCount = model.GuestCount,
            Notes = model.Notes,
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.None,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _bookingRepository.Create(booking);
        await _unitOfWork.SaveChanges(ct);

        return BookingCreateResult.Success(booking.Id);
    }
}


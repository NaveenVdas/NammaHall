using NammaHall.Application.Repository;
using NammaHall.Application.UnitOfWork;
using NammaHall.Domain.Enums;

namespace NammaHall.Application.Handlers.Admin.Booking;

public sealed class AdminBookingUpdateHandler : IAdminBookingUpdateHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminBookingUpdateHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(int bookingId, AdminBookingUpdateModel model, CancellationToken ct)
    {
        var booking = await _bookingRepository.GetById(bookingId, ct);
        if (booking == null)
        {
            return false;
        }

        if (model.Status.HasValue)
        {
            booking.Status = model.Status.Value;
            if (model.Status.Value == BookingStatus.Confirmed)
            {
                booking.ConfirmedAtUtc = DateTime.UtcNow;
            }
            else if (model.Status.Value == BookingStatus.Cancelled)
            {
                booking.CancelledAtUtc = DateTime.UtcNow;
            }
        }

        if (model.AdminComment != null)
        {
            booking.AdminComment = model.AdminComment;
        }

        if (model.CancelReason != null)
        {
            booking.CancelReason = model.CancelReason;
        }

        if (model.AdvanceAmount.HasValue)
        {
            booking.AdvanceAmount = model.AdvanceAmount;
        }

        if (model.PaymentStatus.HasValue)
        {
            booking.PaymentStatus = model.PaymentStatus.Value;
        }

        booking.UpdatedAtUtc = DateTime.UtcNow;

        _bookingRepository.Update(booking);
        await _unitOfWork.SaveChanges(ct);

        return true;
    }
}


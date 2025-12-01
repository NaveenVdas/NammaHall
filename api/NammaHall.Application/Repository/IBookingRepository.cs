using NammaHall.Domain.DomainModel;

namespace NammaHall.Application.Repository;

public interface IBookingRepository
{
    void Create(Booking booking);
    void Update(Booking booking);
    Task<Booking?> GetById(int id, CancellationToken ct);
}


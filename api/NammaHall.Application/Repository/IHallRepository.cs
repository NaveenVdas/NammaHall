using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Application.Repository;

public interface IHallRepository
{
    void Create(Hall hall);
    void Update(Hall hall);
    void Delete(Hall hall);
    Task<Hall?> GetById(int id, CancellationToken ct);
    Task<Hall?> GetByIdWithImages(int id, CancellationToken ct);
}


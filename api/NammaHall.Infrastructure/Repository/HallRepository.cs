using Microsoft.EntityFrameworkCore;
using NammaHall.Application.Repository;
using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Infrastructure.Repository;

public sealed class HallRepository : IHallRepository
{
    private readonly NammaHallDbContext _db;

    public HallRepository(NammaHallDbContext db)
    {
        _db = db;
    }

    public void Create(Hall hall) => _db.Halls.Add(hall);

    public void Update(Hall hall) => _db.Halls.Update(hall);

    public void Delete(Hall hall) => _db.Halls.Remove(hall);

    public async Task<Hall?> GetById(int id, CancellationToken ct) =>
        await _db.Halls.FirstOrDefaultAsync(h => h.Id == id, ct);

    public async Task<Hall?> GetByIdWithImages(int id, CancellationToken ct) =>
        await _db.Halls
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == id, ct);
}


using NammaHall.Application.UnitOfWork;

namespace NammaHall.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly NammaHallDbContext _db;

    public UnitOfWork(NammaHallDbContext db)
    {
        _db = db;
    }

    public async Task SaveChanges(CancellationToken ct)
    {
        await _db.SaveChangesAsync(ct);
    }
}


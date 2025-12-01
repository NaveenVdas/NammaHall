namespace NammaHall.Application.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct);
}


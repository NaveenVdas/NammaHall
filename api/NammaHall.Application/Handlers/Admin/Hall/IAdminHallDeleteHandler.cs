namespace NammaHall.Application.Handlers.Admin.Hall;

public interface IAdminHallDeleteHandler
{
    Task<bool> Handle(int hallId, CancellationToken ct);
}


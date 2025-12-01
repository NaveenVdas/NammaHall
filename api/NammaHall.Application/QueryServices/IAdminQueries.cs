using NammaHall.Domain.DomainModel;

namespace NammaHall.Application.QueryServices;

public interface IAdminQueries
{
    Task<AdminUser?> GetByEmail(string email, CancellationToken ct);
}


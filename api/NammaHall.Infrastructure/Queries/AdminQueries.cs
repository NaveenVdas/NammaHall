using Microsoft.EntityFrameworkCore;
using NammaHall.Application.QueryServices;
using NammaHall.Domain.DomainModel;

namespace NammaHall.Infrastructure.Queries;

public sealed class AdminQueries : IAdminQueries
{
    private readonly NammaHallDbContext _dbContext;

    public AdminQueries(NammaHallDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AdminUser?> GetByEmail(string email, CancellationToken ct) =>
        await _dbContext.AdminUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(au => au.Email == email, ct);
}


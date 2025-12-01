using NammaHall.Application.QueryServices;
using System.Security.Cryptography;
using System.Text;

namespace NammaHall.Application.Handlers.Admin.Auth;

public sealed class AdminLoginHandler : IAdminLoginHandler
{
    private readonly IAdminQueries _adminQueries;

    public AdminLoginHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<AdminLoginResult> Handle(AdminLoginModel model, CancellationToken ct)
    {
        var admin = await _adminQueries.GetByEmail(model.Email, ct);
        if (admin == null || !admin.IsActive)
        {
            return AdminLoginResult.Failure();
        }

        // TODO: Implement proper password hashing verification (BCrypt, Argon2, etc.)
        // For now, this is a placeholder
        string passwordHash = ComputeSha256Hash(model.Password);
        if (admin.PasswordHash != passwordHash)
        {
            return AdminLoginResult.Failure();
        }

        // TODO: Generate JWT token
        string token = "temp-token"; // Replace with actual JWT generation
        return AdminLoginResult.Success(token, admin.DisplayName);
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        StringBuilder builder = new();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}


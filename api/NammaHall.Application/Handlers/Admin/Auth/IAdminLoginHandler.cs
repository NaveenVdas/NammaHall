namespace NammaHall.Application.Handlers.Admin.Auth;

public interface IAdminLoginHandler
{
    Task<AdminLoginResult> Handle(AdminLoginModel model, CancellationToken ct);
}

public sealed class AdminLoginModel
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class AdminLoginResult
{
    public bool IsSuccess { get; init; }
    public string? Token { get; init; }
    public string? DisplayName { get; init; }

    public static AdminLoginResult Success(string token, string displayName) =>
        new() { IsSuccess = true, Token = token, DisplayName = displayName };

    public static AdminLoginResult Failure() =>
        new() { IsSuccess = false };
}


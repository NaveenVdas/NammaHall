namespace NammaHall.Api.Features.Admin.Auth;

public sealed class AdminLoginRequestModel
{
    public required string Email { get; init; }
    public required string Password { get; init; }

    public Application.Handlers.Admin.Auth.AdminLoginModel ToLoginModel() =>
        new()
        {
            Email = Email,
            Password = Password
        };
}


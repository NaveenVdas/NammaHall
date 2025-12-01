namespace NammaHall.Api.Features.Admin.Auth;

public sealed class AdminLoginViewModel
{
    public string Token { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;

    public AdminLoginViewModel(Application.Handlers.Admin.Auth.AdminLoginResult result)
    {
        Token = result.Token ?? string.Empty;
        DisplayName = result.DisplayName ?? string.Empty;
    }
}


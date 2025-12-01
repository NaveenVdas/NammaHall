using Microsoft.AspNetCore.Mvc;
using NammaHall.Api.Features.Admin.Auth;
using NammaHall.Application.Handlers.Admin.Auth;

namespace NammaHall.Api.Controllers;

[ApiController]
[Route("api/admin/auth")]
public sealed class AdminAuthController : ControllerBase
{
    private readonly IAdminLoginHandler _adminLoginHandler;

    public AdminAuthController(IAdminLoginHandler adminLoginHandler)
    {
        _adminLoginHandler = adminLoginHandler;
    }

    [HttpPost("login")]
    [ProducesResponseType<AdminLoginViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AdminLoginRequestModel request, CancellationToken ct)
    {
        var result = await _adminLoginHandler.Handle(request.ToLoginModel(), ct);
        return result.IsSuccess
            ? Ok(new AdminLoginViewModel(result))
            : Unauthorized();
    }
}


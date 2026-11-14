using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Contracts.Auth.Requests;
using MovieFlix.Application.Contracts.Auth.Responses;
using MovieFlix.Application.Services.Auth;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> RegisterAsync(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
        => Ok(await authService.RegisterAsync(request, cancellationToken));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
        => Ok(await authService.LoginAsync(request, cancellationToken));

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
        => Ok(await authService.RefreshTokenAsync(request, cancellationToken));

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync(
        [FromBody] RevokeTokenRequest request,
        CancellationToken cancellationToken = default)
        => await authService.RevokeTokenAsync(request, cancellationToken)
            ? NoContent()
            : BadRequest(new { message = "Invalid or already revoked refresh token." });
}

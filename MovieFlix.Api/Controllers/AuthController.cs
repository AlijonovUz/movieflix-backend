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
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return Ok(result);
    }
}

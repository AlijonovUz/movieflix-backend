using MovieFlix.Application.Contracts.Auth.Requests;
using MovieFlix.Application.Contracts.Auth.Responses;

namespace MovieFlix.Application.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
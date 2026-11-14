using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Auth.Responses;

public record AuthResponse(
    int Id,
    string Username,
    string? Email,
    string? FirstName,
    string? LastName,
    UserRole Role,
    string Token,
    string RefreshToken);

using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Users.Requests;

public record UpdateUserRequest(
    string Username,
    string? Email,
    string? FirstName,
    string? LastName,
    UserRole? Role = null);

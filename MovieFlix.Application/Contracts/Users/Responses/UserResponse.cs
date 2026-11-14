using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Users.Responses;

public record UserResponse(
    int Id,
    string Username,
    string? Email,
    string? FirstName,
    string? LastName,
    UserRole Role,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

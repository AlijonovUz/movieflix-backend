using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Users.Parameters;

public record UserFilterParameters(
    string? Search = null,
    UserRole? Role = null);

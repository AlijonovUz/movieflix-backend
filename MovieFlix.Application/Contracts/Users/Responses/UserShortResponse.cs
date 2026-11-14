namespace MovieFlix.Application.Contracts.Users.Responses;

public record UserShortResponse(
    int Id,
    string Username,
    string? FirstName,
    string? LastName);

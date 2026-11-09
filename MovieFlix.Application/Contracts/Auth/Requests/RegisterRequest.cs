namespace MovieFlix.Application.Contracts.Auth.Requests;

public record RegisterRequest(
    string Username,
    string? Email,
    string Password,
    string? FirstName,
    string? LastName);

namespace MovieFlix.Application.Contracts.Auth.Requests;

public record LoginRequest(
    string UsernameOrEmail,
    string Password);

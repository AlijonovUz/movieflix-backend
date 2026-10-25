namespace MovieFlix.Application.Contracts.Genres.Requests;

public record CreateGenreRequest(string Name, string? Slug);
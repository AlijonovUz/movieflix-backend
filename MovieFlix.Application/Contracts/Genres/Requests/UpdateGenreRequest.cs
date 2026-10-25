namespace MovieFlix.Application.Contracts.Genres.Requests;

public record UpdateGenreRequest(string Name, string? Slug);
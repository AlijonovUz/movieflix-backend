namespace MovieFlix.Application.Contracts.Genres.Responses;

public record GenreResponse(int Id, string Name, string? Slug, DateTime CreatedAt, DateTime? UpdatedAt);
namespace MovieFlix.Application.Contracts.Episodes.Responses;

public record EpisodeResponse(
    int Id,
    int? Number,
    string? Title,
    string VideoUrl,
    int MovieId,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

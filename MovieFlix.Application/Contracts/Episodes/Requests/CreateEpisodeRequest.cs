namespace MovieFlix.Application.Contracts.Episodes.Requests;

public record CreateEpisodeRequest(
    int? Number,
    string? Title,
    string VideoUrl,
    int MovieId);

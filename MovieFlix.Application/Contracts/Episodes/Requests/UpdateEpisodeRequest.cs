namespace MovieFlix.Application.Contracts.Episodes.Requests;

public record UpdateEpisodeRequest(
    int? Number,
    string? Title,
    string VideoUrl);

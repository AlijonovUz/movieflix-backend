using MovieFlix.Application.Contracts.AgeLimits.Responses;
using MovieFlix.Application.Contracts.Genres.Responses;
using MovieFlix.Application.Contracts.ReleaseYears.Responses;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Movies.Responses;

public record MovieListResponse(
    int Id,
    string Title,
    string Slug,
    string PosterUrl,
    int DurationMinutes,
    MovieType MovieType,
    AgeLimitResponse AgeLimit,
    ReleaseYearResponse ReleaseYear,
    IReadOnlyList<GenreResponse> Genres,
    DateTime CreatedAt);

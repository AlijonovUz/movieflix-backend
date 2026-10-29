using MovieFlix.Application.Contracts.AgeLimits.Responses;
using MovieFlix.Application.Contracts.Countries.Responses;
using MovieFlix.Application.Contracts.Genres.Responses;
using MovieFlix.Application.Contracts.ReleaseYears.Responses;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Movies.Responses;

public record MovieDetailResponse(
    int Id,
    string Title,
    string Slug,
    string Description,
    int DurationMinutes,
    string Language,
    MovieType MovieType,
    string PosterUrl,
    string? TrailerUrl,
    AgeLimitResponse AgeLimit,
    ReleaseYearResponse ReleaseYear,
    IReadOnlyList<GenreResponse> Genres,
    IReadOnlyList<CountryResponse> Countries,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

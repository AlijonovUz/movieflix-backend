using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Movies.Parameters;

public record MovieFilterParameters(
    string? Search = null,
    int? GenreId = null,
    int? CountryId = null,
    int? ReleaseYearId = null,
    int? AgeLimitId = null,
    MovieType? MovieType = null);

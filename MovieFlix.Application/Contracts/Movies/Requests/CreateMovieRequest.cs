using Microsoft.AspNetCore.Http;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Movies.Requests;

public record CreateMovieRequest(
    string Title,
    string? Slug,
    string Description,
    int DurationMinutes,
    string Language,
    MovieType MovieType,
    IFormFile? PosterFile,
    string? PosterUrl,
    string? TrailerUrl,
    int AgeLimitId,
    int ReleaseYearId,
    List<int> GenreIds,
    List<int> CountryIds);

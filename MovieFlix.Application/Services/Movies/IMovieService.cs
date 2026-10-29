using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Movies.Parameters;
using MovieFlix.Application.Contracts.Movies.Requests;
using MovieFlix.Application.Contracts.Movies.Responses;

namespace MovieFlix.Application.Services.Movies;

public interface IMovieService
{
    Task<PagedResponse<MovieListResponse>> GetAllAsync(PaginationParameters parameters, 
        MovieFilterParameters filter,
        CancellationToken cancellationToken = default);
    
    Task<MovieDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MovieDetailResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<MovieDetailResponse> CreateAsync(CreateMovieRequest request, CancellationToken cancellationToken = default);
    Task<MovieDetailResponse?> UpdateAsync(int id, UpdateMovieRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
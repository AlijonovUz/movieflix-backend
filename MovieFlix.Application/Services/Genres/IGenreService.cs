using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Genres.Parameters;
using MovieFlix.Application.Contracts.Genres.Requests;
using MovieFlix.Application.Contracts.Genres.Responses;

namespace MovieFlix.Application.Services.Genres;

public interface IGenreService
{
    Task<PagedResponse<GenreResponse>> GetAllAsync(PaginationParameters parameters, GenreFilterParameters filter,
        CancellationToken cancellationToken = default);
    
    Task<GenreResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GenreResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<GenreResponse> CreateAsync(CreateGenreRequest request, CancellationToken cancellationToken = default);
    Task<GenreResponse?> UpdateAsync(int id, UpdateGenreRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
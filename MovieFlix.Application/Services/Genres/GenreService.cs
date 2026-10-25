using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Extensions;
using MovieFlix.Application.Common.Helpers;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Genres.Parameters;
using MovieFlix.Application.Contracts.Genres.Requests;
using MovieFlix.Application.Contracts.Genres.Responses;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.Genres;

public class GenreService(IAppDbContext context) : IGenreService
{
    public async Task<PagedResponse<GenreResponse>> GetAllAsync(
        PaginationParameters parameters, 
        GenreFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var search = filter.Search?.Trim().ToLower();

        var query = context.Genres
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(search), x => 
                x.Name.ToLower().Contains(search!));
        
        return await query
            .ProjectToType<GenreResponse>()
            .ToPagedListAsync(parameters, cancellationToken);
    }

    public async Task<GenreResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await context.Genres
            .ProjectToType<GenreResponse>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<GenreResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default) 
        => await context.Genres
            .ProjectToType<GenreResponse>()
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    

    public async Task<GenreResponse> CreateAsync(CreateGenreRequest request, CancellationToken cancellationToken = default)
    {
        var slug = SlugHelper.ResolveSlug(request.Slug, request.Name);

        var genre = request.Adapt<Genre>();
        genre.Slug = slug;
        
        context.Genres.Add(genre);
        await context.SaveChangesAsync(cancellationToken);
        
        return genre.Adapt<GenreResponse>();
    }

    public async Task<GenreResponse?> UpdateAsync(
        int id,
        UpdateGenreRequest request,
        CancellationToken cancellationToken = default)
    {
        var genre = await context.Genres.FindAsync([id], cancellationToken);
        if (genre is null) return null;

        request.Adapt(genre);
        genre.Slug = SlugHelper.ResolveSlug(request.Slug, request.Name);
        genre.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return genre.Adapt<GenreResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var count = await context.Genres
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return count > 0;
    }
}
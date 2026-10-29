using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Extensions;
using MovieFlix.Application.Common.Helpers;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Movies.Parameters;
using MovieFlix.Application.Contracts.Movies.Requests;
using MovieFlix.Application.Contracts.Movies.Responses;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.Movies;

public class MovieService(IAppDbContext context, IFileService fileService) : IMovieService
{
    public async Task<PagedResponse<MovieListResponse>> GetAllAsync(PaginationParameters parameters, MovieFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var search = filter.Search?.Trim().ToLower();

        var query = context.Movies
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(search), x => 
                x.Title.ToLower().Contains(search!) || x.Description.ToLower().Contains(search!))
            .WhereIf(filter.GenreId.HasValue, x => x.Genres.Any(g => g.Id == filter.GenreId!.Value))
            .WhereIf(filter.CountryId.HasValue, x => x.Countries.Any(c => c.Id == filter.CountryId!.Value))
            .WhereIf(filter.ReleaseYearId.HasValue, x => x.ReleaseYearId == filter.ReleaseYearId!.Value)
            .WhereIf(filter.AgeLimitId.HasValue, x => x.AgeLimitId == filter.AgeLimitId!.Value)
            .WhereIf(filter.MovieType.HasValue, x => x.MovieType == filter.MovieType!.Value);
        
        return await query
            .ProjectToType<MovieListResponse>()
            .ToPagedListAsync(parameters, cancellationToken);
    }

    public async Task<MovieDetailResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await context.Movies
            .AsNoTracking()
            .ProjectToType<MovieDetailResponse>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<MovieDetailResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default) 
        => await context.Movies
            .AsNoTracking()
            .ProjectToType<MovieDetailResponse>()
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

    public async Task<MovieDetailResponse> CreateAsync(CreateMovieRequest request, CancellationToken cancellationToken = default)
    {
        var slug = SlugHelper.ResolveSlug(request.Slug, request.Title);
        
        var genres = await context.Genres
            .Where(g => request.GenreIds.Contains(g.Id))
            .ToListAsync(cancellationToken);
        
        var countries = await context.Countries
            .Where(c => request.CountryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
        
        var movie = request.Adapt<Movie>();
        
        movie.Slug = slug;
        movie.Genres = genres;
        movie.Countries = countries;

        if (request.PosterFile is not null)
        {
            movie.PosterUrl = await fileService.SaveImageAsync(request.PosterFile, "movies", null, cancellationToken)
                ?? string.Empty;
        } 
        else if (!string.IsNullOrWhiteSpace(request.PosterUrl))
        {
            movie.PosterUrl = request.PosterUrl;
        }
        
        context.Movies.Add(movie);
        await context.SaveChangesAsync(cancellationToken);

        return (await GetByIdAsync(movie.Id, cancellationToken))!;
    }

    public async Task<MovieDetailResponse?> UpdateAsync(int id, UpdateMovieRequest request, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies
            .Include(m => m.Genres)
            .Include(m => m.Countries)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        if (movie is null) return null;
        
        var genres = await context.Genres
            .Where(g => request.GenreIds.Contains(g.Id))
            .ToListAsync(cancellationToken);
        
        var countries = await context.Countries
            .Where(c => request.CountryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
        
        request.Adapt(movie);
        movie.Slug = SlugHelper.ResolveSlug(request.Slug, request.Title);
        movie.Genres = genres;
        movie.Countries = countries;
        movie.UpdatedAt = DateTime.UtcNow;

        var oldPosterUrl = movie.PosterUrl;

        if (request.PosterFile is not null)
        {
            movie.PosterUrl = await fileService.SaveImageAsync(request.PosterFile, "movies", null, cancellationToken)
                ?? oldPosterUrl;
        }
        else if (!string.IsNullOrWhiteSpace(request.PosterUrl))
        {
            movie.PosterUrl = oldPosterUrl;
        }

        await context.SaveChangesAsync(cancellationToken);
        return (await GetByIdAsync(movie.Id, cancellationToken))!;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies.FindAsync([id], cancellationToken);
        if (movie is null) return false;
        
        fileService.DeleteImageFile(movie.PosterUrl);

        context.Movies.Remove(movie);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
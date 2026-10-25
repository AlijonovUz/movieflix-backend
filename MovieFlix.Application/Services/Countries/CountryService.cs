using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Extensions;
using MovieFlix.Application.Common.Helpers;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Countries.Parameters;
using MovieFlix.Application.Contracts.Countries.Requests;
using MovieFlix.Application.Contracts.Countries.Responses;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.Countries;

public class CountryService(IAppDbContext context) : ICountryService
{
    public async Task<PagedResponse<CountryResponse>> GetAllAsync(PaginationParameters parameters, CountryFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var search = filter.Search?.Trim().ToLower();

        var query = context.Countries
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(search), x => 
                x.Name.ToLower().Contains(search!));

        return await query
            .ProjectToType<CountryResponse>()
            .ToPagedListAsync(parameters, cancellationToken);
    }

    public async Task<CountryResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Countries
            .ProjectToType<CountryResponse>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<CountryResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default) 
        => await context.Countries
            .ProjectToType<CountryResponse>()
            .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

    public async Task<CountryResponse> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken = default)
    {
        var slug = SlugHelper.ResolveSlug(request.Slug, request.Name);

        var country = request.Adapt<Country>();
        country.Slug = slug;

        context.Countries.Add(country);
        await context.SaveChangesAsync(cancellationToken);
        
        return country.Adapt<CountryResponse>();
    }

    public async Task<CountryResponse?> UpdateAsync(
        int id, 
        UpdateCountryRequest request, 
        CancellationToken cancellationToken = default)
    {
        var country = await context.Countries.FindAsync([id], cancellationToken);
        if (country is null) return null;
        
        request.Adapt(country);
        country.Slug = SlugHelper.ResolveSlug(request.Slug, request.Name);
        country.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);

        return country.Adapt<CountryResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var count = await context.Countries
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return count > 0;
    }
}
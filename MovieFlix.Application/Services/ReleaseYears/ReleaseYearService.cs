using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.ReleaseYears.Requests;
using MovieFlix.Application.Contracts.ReleaseYears.Responses;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.ReleaseYears;

public class ReleaseYearService(IAppDbContext context) : IReleaseYearService
{
    public async Task<IReadOnlyList<ReleaseYearResponse>> GetAllAsync(CancellationToken cancellationToken = default) 
        => await context.ReleaseYears
            .AsNoTracking()
            .OrderByDescending(x => x.Year)
            .ProjectToType<ReleaseYearResponse>()
            .ToListAsync(cancellationToken);

    public async Task<ReleaseYearResponse> CreateAsync(CreateReleaseYearRequest request, CancellationToken cancellationToken = default)
    {
        var releaseYear = request.Adapt<ReleaseYear>();

        context.ReleaseYears.Add(releaseYear);
        await context.SaveChangesAsync(cancellationToken);
        
        return releaseYear.Adapt<ReleaseYearResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var counts = await context.ReleaseYears
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return counts > 0;
    }
}
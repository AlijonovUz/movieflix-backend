using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.Episodes.Requests;
using MovieFlix.Application.Contracts.Episodes.Responses;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.Episodes;

public class EpisodeService(IAppDbContext context) : IEpisodeService
{
    public async Task<EpisodeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Episodes
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectToType<EpisodeResponse>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<EpisodeResponse> CreateAsync(CreateEpisodeRequest request, CancellationToken cancellationToken = default)
    {
        var movieExists = await context.Movies.FindAsync([request.MovieId], cancellationToken);
        if (movieExists is null)
            throw new KeyNotFoundException($"Movie with id {request.MovieId} not found");
        
        var episode = request.Adapt<Episode>();

        context.Episodes.Add(episode);
        await context.SaveChangesAsync(cancellationToken);
        
        return episode.Adapt<EpisodeResponse>();
    }

    public async Task<EpisodeResponse?> UpdateAsync(int id, UpdateEpisodeRequest request, CancellationToken cancellationToken = default)
    {
        var episode = await context.Episodes.FindAsync([id], cancellationToken);
        if (episode is null) return null;

        request.Adapt(episode);
        episode.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return episode.Adapt<EpisodeResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var count = await context.Episodes
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return count > 0;
    }
}

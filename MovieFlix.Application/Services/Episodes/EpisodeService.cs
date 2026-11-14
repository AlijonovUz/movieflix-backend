using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.Episodes.Requests;
using MovieFlix.Application.Contracts.Episodes.Responses;
using MovieFlix.Application.Contracts.Notifications.Requests;
using MovieFlix.Application.Services.Notifications;
using MovieFlix.Domain.Entities;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Services.Episodes;

public class EpisodeService(IAppDbContext context, INotificationService notificationService) : IEpisodeService
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
    
        await SendNotification(episode, cancellationToken);

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

    private async Task SendNotification(Episode episode, CancellationToken cancellationToken = default)
    {
        var isSeries = episode.Movie?.MovieType == MovieType.Series;
        var title = isSeries ? "Yangi qism chiqdi!" : "Yangi film premyerasi!";
        var message = isSeries
            ? $"'{episode.Movie?.Title}' serialining {episode.Number}-qismi chiqdi!"
            : $"'{episode.Movie?.Title}' filmi tomosha qilish uchun platformaga yuklandi!";
        var type = isSeries ? NotificationType.NewEpisode : NotificationType.NewMovie;

         await notificationService.SendBroadcastAsync(new SendNotificationRequest(
            Title: title,
            Message: message,
            Type: type,
            ExtraData: $"{{\"movieId\": {episode.MovieId}, \"episodeId\": {episode.Id}"
        ), cancellationToken);
    }
}

using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.Notifications.Responses;
using MovieFlix.Domain.Entities;
using MovieFlix.Infrastructure.Hubs;

namespace MovieFlix.Infrastructure.BackgroundServices;

public class BroadcastNotificationWorker(
    INotificationQueue queue,
    IServiceScopeFactory scopeFactory,
    IHubContext<NotificationHub, INotificationClient> hubContext,
    ILogger<BroadcastNotificationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("BroadcastNotificationWorker started.");

        await foreach (var request in queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation("BroadcastNotificationWorker has received notification request {Request}", request);

                using var scope = scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

                var userIds = await context.Users
                    .Select(x => x.Id)
                    .ToListAsync(stoppingToken);
                
                var notifications = userIds.Select(id => request.ToNotification(id)).ToList();

                const int batchSize = 500;
                for (var i = 0; i < notifications.Count; i += batchSize)
                {
                    var batch = notifications.Skip(i).Take(batchSize);
                    context.Notifications.AddRange(batch);
                    await context.SaveChangesAsync(stoppingToken);
                }

                var response = request.Adapt<NotificationResponse>() with { CreatedAt = DateTime.UtcNow };
                await hubContext.Clients.All.ReceiveNotification(response);
                
                logger.LogInformation("Broadcast message successfully sent to {count} users", userIds.Count());
            }
            catch (Exception e)
            {
                logger.LogError(e, "BroadcastNotificationWorker error");
            }
        }
    }
}
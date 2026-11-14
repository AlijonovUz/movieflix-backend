using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Extensions;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Notifications.Parameters;
using MovieFlix.Application.Contracts.Notifications.Requests;
using MovieFlix.Application.Contracts.Notifications.Responses;
using MovieFlix.Application.Services.CurrenUser;
using MovieFlix.Domain.Entities;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Services.Notifications;

public class NotificationService(
    IAppDbContext context,
    ICurrentUserService currentUserService,
    INotificationQueue queue,
    INotificationSender notificationSender) : INotificationService
{
    public async Task<PagedResponse<NotificationResponse>> GetMyNotificationsAsync(PaginationParameters parameters, NotificationFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(cancellationToken);

        var query = context.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .WhereIf(filter.IsRead.HasValue, x => x.IsRead == filter.IsRead!.Value)
            .OrderByDescending(x => x.CreatedAt);
        
        return await query
            .ProjectToType<NotificationResponse>()
            .ToPagedListAsync(parameters, cancellationToken);
    }

    public async Task MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        
        var notification = await context.Notifications
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id, cancellationToken);
        
        if (notification is null)
            throw new KeyNotFoundException($"Notification with id {id} not found.");

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkAllAsReadAsync(CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        
        await context.Notifications
            .Where(x => x.UserId == user.Id)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(n => n.IsRead, true), cancellationToken);
    }

    public async Task SendBroadcastAsync(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        await queue.QueueBroadcastAsync(request, cancellationToken);
    }

    public async Task SendToUserAsync(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        if (!request.UserId.HasValue)
            throw new ArgumentException("UserId must be provided when sending a notification to a specific user.");

        var notification = request.ToNotification();
        context.Notifications.Add(notification);
        await context.SaveChangesAsync(cancellationToken);
        
        var response = notification.Adapt<NotificationResponse>();
        await notificationSender.SendToUserAsync(request.UserId.Value, response, cancellationToken);
    }

    public async Task SendToAdminsAsync(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var adminIds = await context.Users
            .AsNoTracking()
            .Where(x => x.Role == UserRole.Admin)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (adminIds.Count == 0) return;

        var notifications = adminIds.Select(id => request.ToNotification(id)).ToList();
        
        context.Notifications.AddRange(notifications);
        await context.SaveChangesAsync(cancellationToken);
        
        var response = request.Adapt<NotificationResponse>() with { CreatedAt = DateTime.UtcNow };
        await notificationSender.SendToUsersAsync(adminIds, response, cancellationToken);
    }

    private async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.UserId
                            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await context.Users.FindAsync([currentUserId], cancellationToken);
        if (user is null)
            throw new KeyNotFoundException("User not found.");

        return user;
    }
}
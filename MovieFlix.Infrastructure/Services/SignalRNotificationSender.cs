using Microsoft.AspNetCore.SignalR;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.Notifications.Responses;
using MovieFlix.Infrastructure.Hubs;

namespace MovieFlix.Infrastructure.Services;

public class SignalRNotificationSender(
    IHubContext<NotificationHub, INotificationClient> hubContext) : INotificationSender
{
    public async Task SendToUserAsync(int userId, NotificationResponse notification,
        CancellationToken cancellationToken = default)
        => await hubContext.Clients.User(userId.ToString()).ReceiveNotification(notification);

    public async Task SendToUsersAsync(IEnumerable<int> userIds, NotificationResponse notification,
        CancellationToken cancellationToken = default)
    {
        var idStrings = userIds.Select(id => id.ToString()).ToList();
        await hubContext.Clients.Users(idStrings).ReceiveNotification(notification);
    }

    public async Task SendToAllAsync(NotificationResponse notification, CancellationToken cancellationToken = default)
        => await hubContext.Clients.All.ReceiveNotification(notification);
}
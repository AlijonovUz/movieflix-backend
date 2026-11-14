using MovieFlix.Application.Contracts.Notifications.Responses;

namespace MovieFlix.Application.Common.Interfaces;

public interface INotificationSender
{
    Task SendToUserAsync(int userId, NotificationResponse notification, CancellationToken cancellationToken = default);
    Task SendToUsersAsync(IEnumerable<int> userIds, NotificationResponse notification, CancellationToken cancellationToken = default);
    Task SendToAllAsync(NotificationResponse notification, CancellationToken cancellationToken = default);
}
using MovieFlix.Application.Contracts.Notifications.Responses;

namespace MovieFlix.Application.Common.Interfaces;

public interface INotificationClient
{
    Task ReceiveNotification(NotificationResponse notification);
}

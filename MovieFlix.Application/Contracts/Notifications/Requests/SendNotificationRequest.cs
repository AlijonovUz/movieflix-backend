using MovieFlix.Domain.Entities;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Notifications.Requests;

public record SendNotificationRequest(
    string Title,
    string Message,
    NotificationType Type = NotificationType.SystemBroadcast,
    int? UserId = null,
    string? ExtraData = null)
{
    public Notification ToNotification(int targetUserId) => new()
    {
        UserId = targetUserId,
        Title = Title,
        Message = Message,
        Type = Type,
        ExtraData = ExtraData
    };

    public Notification ToNotification() => new()
    {
        UserId = UserId,
        Title = Title,
        Message = Message,
        Type = Type,
        ExtraData = ExtraData
    };
}

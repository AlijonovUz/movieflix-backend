using System.Text.Json;
using MovieFlix.Domain.Entities;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Notifications.Requests;

public record SendNotificationRequest(
    string Title,
    string Message,
    NotificationType Type = NotificationType.SystemBroadcast,
    int? UserId = null,
    object? ExtraData = null)
{
    public Notification ToNotification(int targetUserId) => new()
    {
        UserId = targetUserId,
        Title = Title,
        Message = Message,
        Type = Type,
        ExtraData = ExtraData is null ? null : JsonSerializer.SerializeToElement(ExtraData)
    };

    public Notification ToNotification() => new()
    {
        UserId = UserId,
        Title = Title,
        Message = Message,
        Type = Type,
        ExtraData = ExtraData is null ? null : JsonSerializer.SerializeToElement(ExtraData)
    };
}

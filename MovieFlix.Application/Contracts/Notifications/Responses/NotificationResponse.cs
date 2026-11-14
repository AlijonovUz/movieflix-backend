using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Contracts.Notifications.Responses;

public record NotificationResponse(
    int Id,
    string Title,
    string Message,
    string? ExtraData,
    NotificationType Type,
    bool IsRead,
    DateTime CreatedAt);

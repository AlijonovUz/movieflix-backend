using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Notifications.Parameters;
using MovieFlix.Application.Contracts.Notifications.Requests;
using MovieFlix.Application.Contracts.Notifications.Responses;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Services.Notifications;

public interface INotificationService
{
    Task<PagedResponse<NotificationResponse>> GetMyNotificationsAsync(
        PaginationParameters parameters,
        NotificationFilterParameters filter,
        CancellationToken cancellationToken = default);
    
    Task MarkAsReadAsync(int id, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(CancellationToken cancellationToken = default);

    Task SendBroadcastAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken = default);
    
    Task SendToUserAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken = default);

    Task SendToAdminsAsync(
        SendNotificationRequest request,
        CancellationToken cancellationToken = default);
}
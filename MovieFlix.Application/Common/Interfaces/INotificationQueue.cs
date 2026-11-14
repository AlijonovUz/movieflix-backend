using MovieFlix.Application.Contracts.Notifications.Requests;

namespace MovieFlix.Application.Common.Interfaces;

public interface INotificationQueue
{
    ValueTask QueueBroadcastAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);
    IAsyncEnumerable<SendNotificationRequest> ReadAllAsync(CancellationToken cancellationToken = default);
}

using System.Threading.Channels;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.Notifications.Requests;

namespace MovieFlix.Infrastructure.Services;

public class NotificationQueue : INotificationQueue
{
    private readonly Channel<SendNotificationRequest> _channel =
        Channel.CreateUnbounded<SendNotificationRequest>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
            });

    public ValueTask QueueBroadcastAsync(SendNotificationRequest request, CancellationToken cancellationToken = default) 
        => _channel.Writer.WriteAsync(request, cancellationToken);

    public IAsyncEnumerable<SendNotificationRequest> ReadAllAsync(CancellationToken cancellationToken = default) 
        => _channel.Reader.ReadAllAsync(cancellationToken);
}
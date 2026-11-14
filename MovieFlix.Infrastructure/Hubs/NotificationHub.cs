using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MovieFlix.Application.Common.Interfaces;

namespace MovieFlix.Infrastructure.Hubs;

[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
}
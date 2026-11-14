using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Notifications.Parameters;
using MovieFlix.Application.Contracts.Notifications.Requests;
using MovieFlix.Application.Contracts.Notifications.Responses;
using MovieFlix.Application.Services.Notifications;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController(INotificationService service) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResponse<NotificationResponse>>> GetMyNotificationsAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] NotificationFilterParameters filter,
        CancellationToken cancellationToken = default)
        => Ok(await service.GetMyNotificationsAsync(parameters, filter, cancellationToken));

    [HttpPut("{id:int}/read")]
    [Authorize]
    public async Task<IActionResult> MarkAsReadAsync(int id, CancellationToken cancellationToken = default)
    {
        await service.MarkAsReadAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPut("read-all")]
    [Authorize]
    public async Task<IActionResult> MarkAllAsReadAsync(CancellationToken cancellationToken = default)
    {
        await service.MarkAllAsReadAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost("broadcast")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SendBroadcastAsync(
        [FromBody] SendNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        await service.SendBroadcastAsync(request, cancellationToken);
        return Accepted(new { message = "Broadcast notification queued successfully." });
    }
}

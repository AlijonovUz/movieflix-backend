using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Services.CurrenUser;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public int? UserId
    {
        get
        {
            var userClaimId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userClaimId, out var userId) ? userId : null;
        }
    }

    public bool IsAdmin
        => httpContextAccessor.HttpContext?.User.IsInRole(nameof(UserRole.Admin)) ?? false;
}
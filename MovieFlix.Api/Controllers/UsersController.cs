using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Users.Parameters;
using MovieFlix.Application.Contracts.Users.Requests;
using MovieFlix.Application.Contracts.Users.Responses;
using MovieFlix.Application.Services.Users;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var result = await service.GetProfileAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> UpdateProfileAsync(
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.UpdateProfileAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(
        ChangePasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        await service.ChangePasswordAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResponse<UserResponse>>> GetAllAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] UserFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetAllAsync(parameters, filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> UpdateAsync(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

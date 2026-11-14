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
        => Ok(await service.GetProfileAsync(cancellationToken));

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> UpdateProfileAsync(
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
        => Ok(await service.UpdateProfileAsync(request, cancellationToken));

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(
        ChangePasswordRequest request,
        CancellationToken cancellationToken = default)
        => await service.ChangePasswordAsync(request, cancellationToken)
            ? NoContent()
            : BadRequest();

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResponse<UserResponse>>> GetAllAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] UserFilterParameters filter,
        CancellationToken cancellationToken = default)
        => Ok(await service.GetAllAsync(parameters, filter, cancellationToken));

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await service.GetByIdAsync(id, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> UpdateAsync(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
        => await service.UpdateAsync(id, request, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

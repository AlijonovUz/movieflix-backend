using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Contracts.AgeLimits.Requests;
using MovieFlix.Application.Contracts.AgeLimits.Responses;
using MovieFlix.Application.Services.AgeLimits;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgeLimitsController(IAgeLimitService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AgeLimitResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AgeLimitResponse>> CreateAsync(CreateAgeLimitRequest request, CancellationToken cancellationToken = default)
        => StatusCode(StatusCodes.Status201Created, await service.CreateAsync(request, cancellationToken));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

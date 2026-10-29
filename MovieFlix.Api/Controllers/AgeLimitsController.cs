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
    {
        var result = await service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AgeLimitResponse>> CreateAsync(CreateAgeLimitRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
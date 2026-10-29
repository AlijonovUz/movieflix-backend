using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Contracts.ReleaseYears.Requests;
using MovieFlix.Application.Contracts.ReleaseYears.Responses;
using MovieFlix.Application.Services.ReleaseYears;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReleaseYearsController(IReleaseYearService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReleaseYearResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ReleaseYearResponse>> CreateAsync(CreateReleaseYearRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
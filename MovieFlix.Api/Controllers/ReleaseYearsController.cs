using Microsoft.AspNetCore.Authorization;
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
        => Ok(await service.GetAllAsync(cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReleaseYearResponse>> CreateAsync(CreateReleaseYearRequest request, CancellationToken cancellationToken = default)
        => StatusCode(StatusCodes.Status201Created, await service.CreateAsync(request, cancellationToken));
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

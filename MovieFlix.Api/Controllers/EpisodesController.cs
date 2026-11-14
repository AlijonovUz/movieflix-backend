using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Contracts.Episodes.Requests;
using MovieFlix.Application.Contracts.Episodes.Responses;
using MovieFlix.Application.Services.Episodes;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EpisodesController(IEpisodeService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EpisodeResponse?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
        => await service.GetByIdAsync(id, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EpisodeResponse>> CreateAsync(
        CreateEpisodeRequest request,
        CancellationToken cancellationToken = default)
        => await service.CreateAsync(request, cancellationToken) is { } result
            ? CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result)
            : BadRequest();

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EpisodeResponse?>> UpdateAsync(
        int id,
        UpdateEpisodeRequest request,
        CancellationToken cancellationToken = default)
        => await service.UpdateAsync(id, request, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

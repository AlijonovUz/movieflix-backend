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
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EpisodeResponse>> CreateAsync(
        CreateEpisodeRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EpisodeResponse?>> UpdateAsync(
        int id,
        UpdateEpisodeRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Genres.Parameters;
using MovieFlix.Application.Contracts.Genres.Requests;
using MovieFlix.Application.Contracts.Genres.Responses;
using MovieFlix.Application.Services.Genres;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController(IGenreService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<GenreResponse>>> GetAllAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] GenreFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetAllAsync(parameters, filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<GenreResponse?>> GetBySlugAsync(string slug,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetBySlugAsync(slug, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<GenreResponse>> CreateAsync(CreateGenreRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction("GetById", new { id = result.Id}, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenreResponse?>> UpdateAsync(int id, UpdateGenreRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
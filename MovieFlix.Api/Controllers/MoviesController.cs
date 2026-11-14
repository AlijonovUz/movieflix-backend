using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Movies.Parameters;
using MovieFlix.Application.Contracts.Movies.Requests;
using MovieFlix.Application.Contracts.Movies.Responses;
using MovieFlix.Application.Services.Movies;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMovieService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<MovieListResponse>>> GetAllAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] MovieFilterParameters filter,
        CancellationToken cancellationToken = default)
        => Ok(await service.GetAllAsync(parameters, filter, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDetailResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await service.GetByIdAsync(id, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpGet("{slug}")]
    public async Task<ActionResult<MovieDetailResponse?>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => await service.GetBySlugAsync(slug, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MovieDetailResponse>> CreateAsync(
        [FromForm] CreateMovieRequest request,
        CancellationToken cancellationToken = default)
        => await service.CreateAsync(request, cancellationToken) is { } result
            ? CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result)
            : BadRequest();

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MovieDetailResponse?>> UpdateAsync(
        int id,
        [FromForm] UpdateMovieRequest request,
        CancellationToken cancellationToken = default)
        => await service.UpdateAsync(id, request, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

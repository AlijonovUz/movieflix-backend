using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Reviews.Parameters;
using MovieFlix.Application.Contracts.Reviews.Requests;
using MovieFlix.Application.Contracts.Reviews.Responses;
using MovieFlix.Application.Services.Reviews;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(IReviewService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ReviewResponse>>> GetAllAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] ReviewFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetAllAsync(parameters, filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReviewResponse?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReviewResponse>> CreateAsync(
        CreateReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ReviewResponse?>> UpdateAsync(
        int id,
        UpdateReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

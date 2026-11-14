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
        => Ok(await service.GetAllAsync(parameters, filter, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReviewResponse?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
        => await service.GetByIdAsync(id, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReviewResponse>> CreateAsync(
        CreateReviewRequest request,
        CancellationToken cancellationToken = default)
        => await service.CreateAsync(request, cancellationToken) is { } result
            ? CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result)
            : BadRequest();

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ReviewResponse?>> UpdateAsync(
        int id,
        UpdateReviewRequest request,
        CancellationToken cancellationToken = default)
        => await service.UpdateAsync(id, request, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default) 
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

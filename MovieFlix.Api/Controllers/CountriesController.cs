using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Countries.Parameters;
using MovieFlix.Application.Contracts.Countries.Requests;
using MovieFlix.Application.Contracts.Countries.Responses;
using MovieFlix.Application.Services.Countries;

namespace MovieFlix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(ICountryService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<CountryResponse>>> GetAllAsync(
        [FromQuery] PaginationParameters parameters,
        [FromQuery] CountryFilterParameters filter,
        CancellationToken cancellationToken = default)
        => Ok(await service.GetAllAsync(parameters, filter, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CountryResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await service.GetByIdAsync(id, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpGet("{slug}")]
    public async Task<ActionResult<CountryResponse?>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => await service.GetBySlugAsync(slug, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CountryResponse>> CreateAsync(
        CreateCountryRequest request,
        CancellationToken cancellationToken = default)
        => await service.CreateAsync(request, cancellationToken) is { } result
            ? CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result)
            : BadRequest();

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CountryResponse?>> UpdateAsync(
        int id,
        UpdateCountryRequest request,
        CancellationToken cancellationToken = default)
        => await service.UpdateAsync(id, request, cancellationToken) is { } result ? Ok(result) : NotFound();

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

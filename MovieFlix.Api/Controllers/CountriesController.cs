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
    {
        var result = await service.GetAllAsync(parameters, filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CountryResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<CountryResponse?>> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var result = await service.GetBySlugAsync(slug, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CountryResponse>> CreateAsync(CreateCountryRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction("GetById", new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CountryResponse?>> UpdateAsync(int id, UpdateCountryRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
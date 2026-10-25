using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Countries.Parameters;
using MovieFlix.Application.Contracts.Countries.Requests;
using MovieFlix.Application.Contracts.Countries.Responses;

namespace MovieFlix.Application.Services.Countries;

public interface ICountryService
{
    Task<PagedResponse<CountryResponse>> GetAllAsync(PaginationParameters parameters, CountryFilterParameters filter,
        CancellationToken cancellationToken = default);
    
    Task<CountryResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CountryResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<CountryResponse> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken = default);
    Task<CountryResponse?> UpdateAsync(int id, UpdateCountryRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
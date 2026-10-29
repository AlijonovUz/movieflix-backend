using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.ReleaseYears.Requests;
using MovieFlix.Application.Contracts.ReleaseYears.Responses;

namespace MovieFlix.Application.Services.ReleaseYears;

public interface IReleaseYearService
{
    Task<IReadOnlyList<ReleaseYearResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<ReleaseYearResponse> CreateAsync(CreateReleaseYearRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
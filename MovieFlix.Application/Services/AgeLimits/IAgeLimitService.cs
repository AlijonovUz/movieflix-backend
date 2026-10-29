using MovieFlix.Application.Contracts.AgeLimits.Requests;
using MovieFlix.Application.Contracts.AgeLimits.Responses;

namespace MovieFlix.Application.Services.AgeLimits;

public interface IAgeLimitService
{
    Task<IReadOnlyList<AgeLimitResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AgeLimitResponse> CreateAsync(CreateAgeLimitRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
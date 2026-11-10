using MovieFlix.Application.Contracts.Episodes.Requests;
using MovieFlix.Application.Contracts.Episodes.Responses;

namespace MovieFlix.Application.Services.Episodes;

public interface IEpisodeService
{
    Task<EpisodeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EpisodeResponse> CreateAsync(CreateEpisodeRequest request, CancellationToken cancellationToken = default);
    Task<EpisodeResponse?> UpdateAsync(int id, UpdateEpisodeRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.AgeLimits.Requests;
using MovieFlix.Application.Contracts.AgeLimits.Responses;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.AgeLimits;

public class AgeLimitService(IAppDbContext context) : IAgeLimitService
{
    public async Task<IReadOnlyList<AgeLimitResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.AgeLimits
            .AsNoTracking()
            .OrderBy(x => x.Age)
            .ProjectToType<AgeLimitResponse>()
            .ToListAsync(cancellationToken);

    public async Task<AgeLimitResponse> CreateAsync(CreateAgeLimitRequest request,
        CancellationToken cancellationToken = default)
    {
        var ageLimit = request.Adapt<AgeLimit>();

        context.AgeLimits.Add(ageLimit);
        await context.SaveChangesAsync(cancellationToken);
        
        return ageLimit.Adapt<AgeLimitResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var count = await context.AgeLimits
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return count > 0;
    }
}
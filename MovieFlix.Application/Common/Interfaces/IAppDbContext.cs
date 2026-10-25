using Microsoft.EntityFrameworkCore;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Episode> Episodes { get; }
    DbSet<Genre> Genres { get; }
    DbSet<Country> Countries { get; }
    DbSet<Review> Reviews { get; }
    DbSet<User> Users { get; }
    DbSet<AgeLimit> AgeLimits { get; }
    DbSet<ReleaseYear> ReleaseYears { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
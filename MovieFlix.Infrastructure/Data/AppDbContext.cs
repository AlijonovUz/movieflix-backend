using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<AgeLimit> AgeLimits => Set<AgeLimit>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<ReleaseYear> ReleaseYears => Set<ReleaseYear>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
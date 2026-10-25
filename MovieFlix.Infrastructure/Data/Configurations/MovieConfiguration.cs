using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies", t =>
            t.HasCheckConstraint("CK_Movie_DurationMinutes", "DurationMinutes > 0"));
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.HasIndex(x => x.Slug)
            .IsUnique();
        
        builder.Property(x => x.Description)
            .HasMaxLength(2000);
        
        builder.Property(x => x.DurationMinutes)
            .IsRequired();
        
        builder.Property(x => x.PosterUrl)
            .IsRequired()
            .HasMaxLength(5000);
        
        builder.Property(x => x.TrailerUrl)
            .HasMaxLength(5000);

        builder.Property(x => x.Language)
            .HasMaxLength(50);

        builder.Property(x => x.MovieType)
            .HasConversion<string>();
        
        builder.HasOne(x => x.AgeLimit)
            .WithMany(a => a.Movies)
            .HasForeignKey(x => x.AgeLimitId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.ReleaseYear)
            .WithMany(r => r.Movies)
            .HasForeignKey(x => x.ReleaseYearId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Countries)
            .WithMany(c => c.Movies)
            .UsingEntity(t => t.ToTable("MovieCountries"));
        
        builder.HasMany(x => x.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(t => t.ToTable("MovieGenres"));
    }
}
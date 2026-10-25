using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Domain.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Language { get; set; } = string.Empty;
    public MovieType MovieType { get; set; }
    public string PosterUrl { get; set; } = string.Empty;
    public string? TrailerUrl { get; set; }
    
    public int AgeLimitId { get; set; }
    public AgeLimit? AgeLimit { get; set; }
    
    public int ReleaseYearId { get; set; }
    public ReleaseYear? ReleaseYear { get; set; }

    public ICollection<Country> Countries { get; set; } = new List<Country>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
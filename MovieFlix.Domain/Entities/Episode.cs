namespace MovieFlix.Domain.Entities;

public class Episode : BaseEntity
{
    public int? Number { get; set; }
    public string? Title { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    
    public int MovieId { get; set; }
    public Movie? Movie { get; set; } 
}
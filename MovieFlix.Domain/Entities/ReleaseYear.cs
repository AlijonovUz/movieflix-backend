namespace MovieFlix.Domain.Entities;

public class ReleaseYear : BaseEntity
{
    public int Year { get; set; }
    
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
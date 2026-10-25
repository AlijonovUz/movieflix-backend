namespace MovieFlix.Domain.Entities;

public class AgeLimit : BaseEntity
{
    public int Age { get; set; }
    
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
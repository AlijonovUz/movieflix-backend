namespace MovieFlix.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
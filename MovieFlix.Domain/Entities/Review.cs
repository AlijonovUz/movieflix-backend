namespace MovieFlix.Domain.Entities;

public class Review : BaseEntity
{
    public int? Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int LikesCount { get; set; } = 0;
    public int DislikesCount { get; set; } = 0;
    
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
    
    public int? ParentId { get; set; }
    public Review? Parent { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public ICollection<Review> Replies { get; set; } = new List<Review>();
}
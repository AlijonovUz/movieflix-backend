using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Domain.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ExtraData { get; set; }
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; } = false;
    
    public int? UserId { get; set; }
    public User? User { get; set; }
}
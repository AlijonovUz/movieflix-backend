namespace MovieFlix.Application.Services.CurrenUser;

public interface ICurrentUserService
{
    int? UserId { get; }
    bool IsAdmin { get; }
}
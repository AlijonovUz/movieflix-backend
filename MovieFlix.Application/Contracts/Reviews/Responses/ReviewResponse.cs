using MovieFlix.Application.Contracts.Users.Responses;

namespace MovieFlix.Application.Contracts.Reviews.Responses;

public record ReviewResponse(
    int Id,
    int MovieId,
    UserShortResponse User,
    int? Rating,
    string Comment,
    int? ParentId,
    IReadOnlyList<ReviewResponse>? Replies,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

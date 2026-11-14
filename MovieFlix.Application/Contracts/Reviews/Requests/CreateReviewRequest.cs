namespace MovieFlix.Application.Contracts.Reviews.Requests;

public record CreateReviewRequest(
    int MovieId,
    string Comment,
    int? Rating = null,
    int? ParentId = null);

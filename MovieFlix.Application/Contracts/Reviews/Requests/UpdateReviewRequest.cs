namespace MovieFlix.Application.Contracts.Reviews.Requests;

public record UpdateReviewRequest(
    string Comment,
    int? Rating = null);

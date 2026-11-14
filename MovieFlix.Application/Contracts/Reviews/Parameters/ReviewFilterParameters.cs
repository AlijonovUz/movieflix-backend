namespace MovieFlix.Application.Contracts.Reviews.Parameters;

public record ReviewFilterParameters(
    int? MovieId = null,
    int? UserId = null,
    int? MinRating = null,
    int? MaxRating = null);

namespace MovieFlix.Application.Contracts.Countries.Responses;

public record CountryResponse(int Id, string Name, string? Slug, DateTime CreatedAt, DateTime? UpdatedAt);
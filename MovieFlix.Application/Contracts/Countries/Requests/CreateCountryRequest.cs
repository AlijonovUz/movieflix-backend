namespace MovieFlix.Application.Contracts.Countries.Requests;

public record CreateCountryRequest(string Name, string? Slug);
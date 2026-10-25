namespace MovieFlix.Application.Contracts.Countries.Requests;

public record UpdateCountryRequest(string Name, string? Slug);
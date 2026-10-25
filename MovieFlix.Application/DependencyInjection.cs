using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieFlix.Application.Services.Countries;
using MovieFlix.Application.Services.Genres;

namespace MovieFlix.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        AddApplicationServices(services);

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<ICountryService, CountryService>();
        
        return services;
    }
}
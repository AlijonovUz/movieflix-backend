using System.Reflection;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MovieFlix.Application.Contracts.Reviews.Responses;
using MovieFlix.Domain.Entities;
using MovieFlix.Application.Services.AgeLimits;
using MovieFlix.Application.Services.Auth;
using MovieFlix.Application.Services.Countries;
using MovieFlix.Application.Services.CurrenUser;
using MovieFlix.Application.Services.Episodes;
using MovieFlix.Application.Services.Genres;
using MovieFlix.Application.Services.Movies;
using MovieFlix.Application.Services.Notifications;
using MovieFlix.Application.Services.ReleaseYears;
using MovieFlix.Application.Services.Reviews;
using MovieFlix.Application.Services.Users;

namespace MovieFlix.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        TypeAdapterConfig<Review, ReviewResponse>.NewConfig().MaxDepth(3);

        AddApplicationServices(services);

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IAgeLimitService, AgeLimitService>();
        services.AddScoped<IReleaseYearService, ReleaseYearService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IEpisodeService, EpisodeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        
        return services;
    }
}
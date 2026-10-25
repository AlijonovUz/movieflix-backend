using FluentValidation.AspNetCore;
using MovieFlix.Api.Middlewares;

namespace MovieFlix.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        
        // Kichik harfli URL mashrutlar
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        
        // Global Exception Handler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        // FluentValidation avtomatik tekshiruvi
        services.AddFluentValidationAutoValidation();
        
        return services;
    }
}
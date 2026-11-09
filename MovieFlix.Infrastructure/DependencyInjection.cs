using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Infrastructure.Authentication;
using MovieFlix.Infrastructure.Data;
using MovieFlix.Infrastructure.Services;

namespace MovieFlix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddAuthenticationInfrastructure(configuration);
        services.AddStorageInfrastructure();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Baza ulanishi topilmadi.");
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }

    private static IServiceCollection AddAuthenticationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

    private static IServiceCollection AddStorageInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}

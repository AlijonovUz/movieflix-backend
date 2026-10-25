using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Infrastructure.Data;

namespace MovieFlix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(
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
}
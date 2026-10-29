using MovieFlix.Application.Commands.Cli;
using MovieFlix.Application.Common.Interfaces;

namespace MovieFlix.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<bool> RunCliCommandsAsync(this WebApplication app, string[] args)
    {
        if (args.Contains("admin"))
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            await AdminCliManager.RunAsync(context);
            return true;
        }
        
        return false;
    }
    
    public static WebApplication UseApiConfiguration(this WebApplication app)
    {
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}
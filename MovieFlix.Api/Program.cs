using MovieFlix.Api.Extensions;
using MovieFlix.Application;
using MovieFlix.Infrastructure;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Har bir qatlam o'z servislarini ro'yxatdan o'tkazadi
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// CLI buyruqlarini tekshirish (--admin)
if (await app.RunCliCommandsAsync(args))
    return;

// API pipeline sozlamalari
app.UseApiConfiguration();

app.Run();

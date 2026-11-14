# 1. Build bosqichi (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Qatlamlarni keshlashtirish (Docker layer caching) uchun loyiha fayllarini ko'chirish
COPY ["MovieFlix.slnx", "./"]
COPY ["MovieFlix.Domain/MovieFlix.Domain.csproj", "MovieFlix.Domain/"]
COPY ["MovieFlix.Application/MovieFlix.Application.csproj", "MovieFlix.Application/"]
COPY ["MovieFlix.Infrastructure/MovieFlix.Infrastructure.csproj", "MovieFlix.Infrastructure/"]
COPY ["MovieFlix.Api/MovieFlix.Api.csproj", "MovieFlix.Api/"]

# Bog'liqliklarni tiklash (Restore)
RUN dotnet restore "MovieFlix.Api/MovieFlix.Api.csproj"

# Qolgan barcha manba kodlarni ko'chirish
COPY . .

# Release rejimida publish qilish
WORKDIR "/src/MovieFlix.Api"
RUN dotnet publish "MovieFlix.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Ishga tushirish bosqichi (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Ma'lumotlar bazasi va statik fayllar uchun papka
RUN mkdir -p /app/data /app/wwwroot

# Publish qilingan fayllarni ko'chirish
COPY --from=build /app/publish .

# Standart port va sozlamalar
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=Asia/Tashkent

ENTRYPOINT ["dotnet", "MovieFlix.Api.dll"]

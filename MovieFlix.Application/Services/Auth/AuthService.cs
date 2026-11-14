using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Contracts.Auth.Requests;
using MovieFlix.Application.Contracts.Auth.Responses;
using MovieFlix.Domain.Entities;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Services.Auth;

public class AuthService(IAppDbContext context, IJwtTokenGenerator generator) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var usernameExists = await context.Users
            .AnyAsync(x => x.Username.ToLower() == request.Username.ToLower(), cancellationToken);

        if (usernameExists)
            throw new BadHttpRequestException("This username is already taken");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await context.Users
                .AnyAsync(x => x.Email != null && x.Email.ToLower() == request.Email.ToLower(), cancellationToken);

            if (emailExists)
                throw new BadHttpRequestException("This email is already taken");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var user = request.Adapt<User>();
        user.PasswordHash = passwordHash;
        user.Role = UserRole.User;
        user.CreatedAt = DateTime.UtcNow;

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        var token = generator.Generate(user);
        
        var response = user.Adapt<AuthResponse>() with { Token = token };
        return response;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedInput = request.UsernameOrEmail.Trim().ToLower();
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Username.ToLower() == normalizedInput 
                                      || (x.Email != null && x.Email.ToLower() == normalizedInput), cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadHttpRequestException("Incorrect username or password");

        var token = generator.Generate(user);
        
        var response = user.Adapt<AuthResponse>() with { Token = token };
        return response;
    }
}
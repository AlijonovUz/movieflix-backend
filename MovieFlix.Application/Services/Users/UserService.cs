using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Extensions;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Users.Parameters;
using MovieFlix.Application.Contracts.Users.Requests;
using MovieFlix.Application.Contracts.Users.Responses;
using MovieFlix.Application.Services.CurrenUser;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Application.Services.Users;

public class UserService(IAppDbContext context, ICurrentUserService currentUserService) : IUserService
{
    public async Task<UserResponse> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user.Adapt<UserResponse>();
    }

    public async Task<UserResponse> UpdateProfileAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        
        await EnsureUniqueAsync(user.Id, request.Username, request.Email, cancellationToken);

        var oldRole = user.Role;
        request.Adapt(user);
        
        user.Role = oldRole;

        user.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);
        return user.Adapt<UserResponse>();
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        
        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new BadHttpRequestException("Current password is incorrect.");
        
        if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            throw new BadHttpRequestException("New password cannot be the current password.");
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PagedResponse<UserResponse>> GetAllAsync(PaginationParameters parameters, UserFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var search = filter.Search?.Trim().ToLower();

        var query = context.Users
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(search), x => 
                x.Username.ToLower().Contains(search!) ||
                (x.Email != null && x.Email.ToLower().Contains(search!)) ||
                (x.FirstName != null && x.FirstName.ToLower().Contains(search!)) ||
                (x.LastName != null && x.LastName.ToLower().Contains(search!)))
            .WhereIf(filter.Role.HasValue, x => x.Role == filter.Role!.Value)
            .OrderByDescending(x => x.CreatedAt);
        
        return await query
            .ProjectToType<UserResponse>()
            .ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectToType<UserResponse>()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync([id], cancellationToken);
        if (user is null) return null;
        
        await EnsureUniqueAsync(user.Id, request.Username, request.Email, cancellationToken);
        
        var oldRole = user.Role;
        request.Adapt(user);
        
        if (!request.Role.HasValue)
            user.Role = oldRole;
        
        user.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);
        return user.Adapt<UserResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id == currentUserService.UserId)
            throw new InvalidOperationException("You cannot delete yourself.");

        var count = await context.Users
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return count > 0;
    }

    private async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.UserId
                            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await context.Users.FindAsync([currentUserId], cancellationToken);
        if (user is null)
            throw new KeyNotFoundException("User not found.");

        return user;
    }

    private async Task EnsureUniqueAsync(int userId, string username, string? email,
        CancellationToken cancellationToken)
    {
        var normalizedUsername = username.Trim().ToLower();
        var isUsernameTaken = await context.Users
            .AnyAsync(x => x.Id != userId && x.Username.ToLower() == normalizedUsername, cancellationToken);
        if (isUsernameTaken)
            throw new BadHttpRequestException("This username is already taken.");

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = email.Trim().ToLower();
            var isEmailTaken = await context.Users
                .AnyAsync(x => x.Id != userId 
                               && x .Email != null 
                               && x.Email.ToLower() == normalizedEmail, 
                    cancellationToken);
            if (isEmailTaken)
                throw new BadHttpRequestException("This email is already taken.");
        }
    }
    
}
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Users.Parameters;
using MovieFlix.Application.Contracts.Users.Requests;
using MovieFlix.Application.Contracts.Users.Responses;

namespace MovieFlix.Application.Services.Users;

public interface IUserService
{
    Task<UserResponse> GetProfileAsync(CancellationToken cancellationToken = default);
    Task<UserResponse> UpdateProfileAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
    
    Task<PagedResponse<UserResponse>> GetAllAsync(
        PaginationParameters parameters,
        UserFilterParameters filter,
        CancellationToken cancellationToken = default);
    Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
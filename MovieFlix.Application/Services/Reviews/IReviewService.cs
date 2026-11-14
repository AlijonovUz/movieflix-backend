using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Reviews.Parameters;
using MovieFlix.Application.Contracts.Reviews.Requests;
using MovieFlix.Application.Contracts.Reviews.Responses;

namespace MovieFlix.Application.Services.Reviews;

public interface IReviewService
{
    Task<PagedResponse<ReviewResponse>> GetAllAsync(
        PaginationParameters parameters,
        ReviewFilterParameters filter,
        CancellationToken cancellationToken = default);
    
    Task<ReviewResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ReviewResponse> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<ReviewResponse?> UpdateAsync(int id, UpdateReviewRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
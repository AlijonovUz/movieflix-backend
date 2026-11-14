using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Extensions;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;
using MovieFlix.Application.Contracts.Notifications.Requests;
using MovieFlix.Application.Contracts.Reviews.Parameters;
using MovieFlix.Application.Contracts.Reviews.Requests;
using MovieFlix.Application.Contracts.Reviews.Responses;
using MovieFlix.Application.Services.CurrenUser;
using MovieFlix.Application.Services.Notifications;
using MovieFlix.Domain.Entities;
using MovieFlix.Domain.Entities.Enums;

namespace MovieFlix.Application.Services.Reviews;

public class ReviewService(
    IAppDbContext context, 
    ICurrentUserService currentUserService,
    INotificationService notificationService) : IReviewService
{
    public async Task<PagedResponse<ReviewResponse>> GetAllAsync(
        PaginationParameters parameters, 
        ReviewFilterParameters filter, 
        CancellationToken cancellationToken)
    {
        var query = context.Reviews
            .AsNoTracking()
            .Where(x => x.ParentId == null)
            .WhereIf(filter.MovieId.HasValue, x => x.MovieId == filter.MovieId!.Value)
            .WhereIf(filter.UserId.HasValue, x => x.UserId == filter.UserId!.Value)
            .WhereIf(filter.MinRating.HasValue, x => x.Rating >= filter.MinRating!.Value)
            .WhereIf(filter.MaxRating.HasValue, x => x.Rating <= filter.MaxRating!.Value)
            .OrderByDescending(x => x.CreatedAt);
        
        return await query
            .ProjectToType<ReviewResponse>()
            .ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<ReviewResponse?> GetByIdAsync(int id, CancellationToken cancellationToken) 
        => await context.Reviews
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectToType<ReviewResponse>()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId
                            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == request.MovieId, cancellationToken);
        if (movie is null)
            throw new KeyNotFoundException($"Movie with id {request.MovieId} not found.");

        Review? parent = null;
        if (request.ParentId.HasValue)
        {
            parent = await context.Reviews
                .FirstOrDefaultAsync(x => x.Id == request.ParentId.Value, cancellationToken);

            if (parent is null)
                throw new KeyNotFoundException($"Parent review with id {request.ParentId.Value} not found.");

            if (parent.MovieId != request.MovieId)
                throw new InvalidOperationException("Parent review belongs to a different movie.");

            if (parent.ParentId != null)
                throw new InvalidOperationException("Nested replies beyond 1 level are not supported.");
        }

        var review = request.Adapt<Review>();
        review.UserId = currentUserId;

        context.Reviews.Add(review);
        await context.SaveChangesAsync(cancellationToken);

        await SendNotificationAsync(movie, review, parent, currentUserId, cancellationToken);

        return (await GetByIdAsync(review.Id, cancellationToken))!;
    }

    public async Task<ReviewResponse?> UpdateAsync(int id, UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var review = await context.Reviews.FindAsync([id], cancellationToken);
        if (review is null) return null;

        if (review.UserId != currentUserService.UserId && !currentUserService.IsAdmin)
            throw new UnauthorizedAccessException("You are not allowed to update this review.");

        request.Adapt(review);
        review.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return (await GetByIdAsync(review.Id, cancellationToken))!;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var review = await context.Reviews.FindAsync([id], cancellationToken);
        if (review is null) return false;

        if (review.UserId != currentUserService.UserId && !currentUserService.IsAdmin)
            throw new UnauthorizedAccessException("You are not allowed to delete this review.");

        var replies = await context.Reviews
            .Where(x => x.ParentId == id)
            .ToListAsync(cancellationToken);
        
        if (replies.Count > 0)
            context.Reviews.RemoveRange(replies);

        context.Reviews.Remove(review);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task SendNotificationAsync(
        Movie movie,
        Review review,
        Review? parent,
        int currentUserId,
        CancellationToken cancellationToken)
    {
        if (parent is not null)
        {
            if (parent.UserId != currentUserId)
            {
                await notificationService.SendToUserAsync(new SendNotificationRequest(
                    Title: "Sharhingizga javob berildi",
                    Message: $"'{movie.Title}' filmidagi sharhingizga yangi javob qoldirildi.",
                    Type: NotificationType.ReviewReply,
                    UserId: parent.UserId,
                    ExtraData: $"{{\"movieId\": {review.MovieId}, \"reviewId\": {review.Id}, \"parentId\": {parent.Id}}}"
                ), cancellationToken);
            }
        }
        else
        {
            await notificationService.SendToAdminsAsync(new SendNotificationRequest(
                Title: "Yangi sharh qoldirildi",
                Message: $"'{movie.Title}' filmiga yangi sharh qoldirildi.",
                Type: NotificationType.NewReview,
                ExtraData: $"{{\"movieId\": {review.MovieId}, \"reviewId\": {review.Id}}}"
            ), cancellationToken);
        }
    }
}
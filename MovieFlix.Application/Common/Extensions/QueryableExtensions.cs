using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Application.Common.Pagination.Parameters;
using MovieFlix.Application.Common.Pagination.Responses;

namespace MovieFlix.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResponse<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var page = pageNumber < 1 ? 1 : pageNumber;
        var size = pageSize is < 1 or > 50 ? 10 : pageSize;

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        
        return new PagedResponse<T>(items, totalCount, page, size);
    }

    public static Task<PagedResponse<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        PaginationParameters parameters,
        CancellationToken cancellationToken = default) 
        => query.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
        => condition ? query.Where(predicate) : query;
    
}
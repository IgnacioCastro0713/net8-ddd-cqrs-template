using Microsoft.EntityFrameworkCore;

namespace Application.Extensions;

public static class PaginatedExtensions
{
    public static Task<PaginatedOf<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        return PaginatedOf<TDestination>.CreateAsync(
            queryable.AsNoTracking(),
            pageNumber,
            pageSize,
            cancellationToken);
    }

    public static Task<PaginatedOf<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        Paginated paginated,
        CancellationToken cancellationToken = default) where TDestination : class
    {
        return PaginatedOf<TDestination>.CreateAsync(
            queryable.AsNoTracking(),
            paginated.PageNumber,
            paginated.PageSize,
            cancellationToken);
    }
}
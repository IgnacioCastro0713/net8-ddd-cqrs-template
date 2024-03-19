using Microsoft.EntityFrameworkCore;

namespace Domain.Shared;

public sealed record Paginated
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortColumn { get; set; } = string.Empty;
    public string? Direction { get; set; } = "ASC";
}

public class PaginatedOf<T>(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
{
    public IReadOnlyCollection<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalCount { get; } = count;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedOf<T>> CreateAsync(
        IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await queryable.CountAsync(cancellationToken);
        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedOf<T>(items, count, pageNumber, pageSize);
    }
}
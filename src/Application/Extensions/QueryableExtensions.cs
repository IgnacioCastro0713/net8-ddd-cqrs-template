using System.Linq.Expressions;

namespace Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string? columnName, string? direction = "ASC")
    {
        if (string.IsNullOrEmpty(columnName)) return source;

        var parameter = Expression.Parameter(source.ElementType, "");
        var property = Expression.Property(parameter, columnName);
        var lambda = Expression.Lambda(property, parameter);
        var methodName = string.IsNullOrEmpty(direction) || direction.Equals("ASC", StringComparison.CurrentCultureIgnoreCase)
            ? "OrderBy"
            : "OrderByDescending";
        var methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [source.ElementType, property.Type],
            source.Expression,
            Expression.Quote(lambda));

        return source.Provider.CreateQuery<T>(methodCallExpression);
    }
}
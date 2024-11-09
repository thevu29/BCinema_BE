using System.Linq.Expressions;

namespace BCinema.Application.Helpers;

public static class QueryableHelper
{
    public static IQueryable<T> ApplyDynamicSorting<T>(
        this IQueryable<T> query,
        string? sortBy,
        string? sortOrder) where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
        
        if (string.IsNullOrWhiteSpace(sortBy)) return query;

        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            
            var property = Expression.Property(parameter, sortBy);
            
            var lambda = Expression.Lambda(property, parameter);
            
            var isAscending = !string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            
            var methodName = isAscending ? "OrderBy" : "OrderByDescending";
            
            var orderByMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);
            
            var result = orderByMethod.Invoke(null, new object[] { query, lambda });

            return result as IQueryable<T> ?? query;
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or MissingMemberException)
        {
            return query;
        }
    }
}
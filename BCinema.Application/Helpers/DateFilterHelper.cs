using System.Linq.Expressions;
using BCinema.Application.Exceptions;

namespace BCinema.Application.Helpers;

public static class DateFilterHelper
{
    public static IQueryable<T> FilterByDate<T>(
        this IQueryable<T> query,
        string dateFilter,
        Expression<Func<T, DateTime>> dateSelector)
    {
        if (string.IsNullOrWhiteSpace(dateFilter))
            return query;

        if (dateFilter.Contains("to"))
        {
            return FilterDateRange(query, dateFilter, dateSelector);
        }
        
        if (dateFilter.StartsWith('>'))
        {
            return FilterAfterDate(query, dateFilter, dateSelector);
        }
        
        if (dateFilter.StartsWith('<'))
        {
            return FilterBeforeDate(query, dateFilter, dateSelector);
        }
        
        return FilterExactDate(query, dateFilter, dateSelector);
    }

    private static IQueryable<T> FilterDateRange<T>(
        IQueryable<T> query,
        string dateFilter,
        Expression<Func<T, DateTime>> dateSelector)
    {
        var dates = dateFilter.Split("to");
        if (dates.Length != 2)
            throw new BadRequestException("Invalid date range format. Use 'yyyy-MM-dd to yyyy-MM-dd'");

        if (!TryParseDate(dates[0].Trim(), out var startDate) || !TryParseDate(dates[1].Trim(), out var endDate))
            throw new BadRequestException("Invalid date format in range. Use yyyy-MM-dd");

        var parameter = dateSelector.Parameters[0];
        var memberAccess = dateSelector.Body;

        var startDateConstant = Expression.Constant(startDate.ToUniversalTime());
        var endDateConstant = Expression.Constant(endDate.ToUniversalTime());

        var greaterThanOrEqual = Expression.GreaterThanOrEqual(memberAccess, startDateConstant);
        var lessThanOrEqual = Expression.LessThanOrEqual(memberAccess, endDateConstant);
        var combined = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
        return query.Where(lambda);
    }

    private static IQueryable<T> FilterAfterDate<T>(
        IQueryable<T> query,
        string dateFilter,
        Expression<Func<T, DateTime>> dateSelector)
    {
        if (!TryParseDate(dateFilter[1..], out var afterDate))
            throw new BadRequestException("Invalid date format. Use >yyyy-MM-dd");

        var parameter = dateSelector.Parameters[0];
        var memberAccess = dateSelector.Body;
        var dateConstant = Expression.Constant(afterDate.ToUniversalTime());
        var greaterThan = Expression.GreaterThan(memberAccess, dateConstant);
        var lambda = Expression.Lambda<Func<T, bool>>(greaterThan, parameter);

        return query.Where(lambda);
    }

    private static IQueryable<T> FilterBeforeDate<T>(
        IQueryable<T> query,
        string dateFilter,
        Expression<Func<T, DateTime>> dateSelector)
    {
        if (!TryParseDate(dateFilter[1..], out var beforeDate))
            throw new BadRequestException("Invalid date format. Use <yyyy-MM-dd");

        var parameter = dateSelector.Parameters[0];
        var memberAccess = dateSelector.Body;
        var dateConstant = Expression.Constant(beforeDate.ToUniversalTime());
        var lessThan = Expression.LessThan(memberAccess, dateConstant);
        var lambda = Expression.Lambda<Func<T, bool>>(lessThan, parameter);

        return query.Where(lambda);
    }

    private static IQueryable<T> FilterExactDate<T>(
        IQueryable<T> query,
        string dateFilter,
        Expression<Func<T, DateTime>> dateSelector)
    {
        if (!TryParseDate(dateFilter, out var exactDate))
            throw new BadRequestException("Invalid date format. Use yyyy-MM-dd");

        var parameter = dateSelector.Parameters[0];
        var memberAccess = Expression.Property(dateSelector.Body, "Date");
        var dateConstant = Expression.Constant(exactDate.ToUniversalTime().Date);
        var equal = Expression.Equal(memberAccess, dateConstant);
        var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

        return query.Where(lambda);
    }

    private static bool TryParseDate(string date, out DateTime result)
    {
        return DateTime.TryParseExact(
            date.Trim(),
            "yyyy-MM-dd",
            null,
            System.Globalization.DateTimeStyles.AssumeUniversal,
            out result);
    }
}
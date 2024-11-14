using System.Linq.Expressions;
using BCinema.Application.Exceptions;

namespace BCinema.Application.Helpers;

public static class NumberFilterHelper
{
    public static IQueryable<T> FilterByNumber<T, TNumber>(
        this IQueryable<T> query,
        string numberFilter,
        Expression<Func<T, TNumber>> numberSelector) where TNumber : struct, IComparable<TNumber>
    {
        if (string.IsNullOrWhiteSpace(numberFilter))
            return query;

        if (numberFilter.Contains("to"))
        {
            return FilterNumberRange(query, numberFilter, numberSelector);
        }
        
        if (numberFilter.StartsWith('>'))
        {
            return FilterGreaterThan(query, numberFilter, numberSelector);
        }
        
        if (numberFilter.StartsWith('<'))
        {
            return FilterLessThan(query, numberFilter, numberSelector);
        }
        
        return FilterExactNumber(query, numberFilter, numberSelector);
    }

    private static IQueryable<T> FilterNumberRange<T, TNumber>(
        IQueryable<T> query,
        string numberFilter,
        Expression<Func<T, TNumber>> numberSelector) where TNumber : struct, IComparable<TNumber>
    {
        var numbers = numberFilter.Split("to");
        if (numbers.Length != 2)
            throw new BadRequestException("Invalid number range format. Use 'number to number'");

        if (!TryParseNumber<TNumber>(numbers[0].Trim(), out var startNumber) ||
            !TryParseNumber<TNumber>(numbers[1].Trim(), out var endNumber))
        {
            throw new BadRequestException($"Invalid number format in range. Numbers must be of type {typeof(TNumber).Name}");
        }
        
        var parameter = numberSelector.Parameters[0];
        var memberAccess = numberSelector.Body;

        var startNumberConstant = Expression.Constant(startNumber);
        var endNumberConstant = Expression.Constant(endNumber);

        var greaterThanOrEqual = Expression.GreaterThanOrEqual(memberAccess, startNumberConstant);
        var lessThanOrEqual = Expression.LessThanOrEqual(memberAccess, endNumberConstant);
        var combined = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
        return query.Where(lambda);
    }

    private static IQueryable<T> FilterGreaterThan<T, TNumber>(
        IQueryable<T> query,
        string numberFilter,
        Expression<Func<T, TNumber>> numberSelector) where TNumber : struct, IComparable<TNumber>
    {
        if (!TryParseNumber<TNumber>(numberFilter[1..], out var number))
            throw new BadRequestException($"Invalid number format. Use >{typeof(TNumber).Name}");

        var parameter = numberSelector.Parameters[0];
        var memberAccess = numberSelector.Body;
        var numberConstant = Expression.Constant(number);
        var greaterThan = Expression.GreaterThan(memberAccess, numberConstant);
        var lambda = Expression.Lambda<Func<T, bool>>(greaterThan, parameter);

        return query.Where(lambda);
    }

    private static IQueryable<T> FilterLessThan<T, TNumber>(
        IQueryable<T> query,
        string numberFilter,
        Expression<Func<T, TNumber>> numberSelector) where TNumber : struct, IComparable<TNumber>
    {
        if (!TryParseNumber<TNumber>(numberFilter[1..], out var number))
            throw new BadRequestException($"Invalid number format. Use <{typeof(TNumber).Name}");

        var parameter = numberSelector.Parameters[0];
        var memberAccess = numberSelector.Body;
        var numberConstant = Expression.Constant(number);
        var lessThan = Expression.LessThan(memberAccess, numberConstant);
        var lambda = Expression.Lambda<Func<T, bool>>(lessThan, parameter);

        return query.Where(lambda);
    }

    private static IQueryable<T> FilterExactNumber<T, TNumber>(
        IQueryable<T> query,
        string numberFilter,
        Expression<Func<T, TNumber>> numberSelector) where TNumber : struct, IComparable<TNumber>
    {
        if (!TryParseNumber<TNumber>(numberFilter, out var number))
            throw new BadRequestException($"Invalid number format. Number must be of type {typeof(TNumber).Name}");

        var parameter = numberSelector.Parameters[0];
        var memberAccess = numberSelector.Body;
        var numberConstant = Expression.Constant(number);
        var equal = Expression.Equal(memberAccess, numberConstant);
        var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

        return query.Where(lambda);
    }

    private static bool TryParseNumber<TNumber>(string value, out TNumber result) where TNumber : struct
    {
        value = value.Trim();
        try
        {
            if (typeof(TNumber) == typeof(int))
            {
                var success = int.TryParse(value, out var intResult);
                result = success ? (TNumber)(object)intResult : default;
                return success;
            }
            if (typeof(TNumber) == typeof(decimal))
            {
                var success = decimal.TryParse(value, out var decimalResult);
                result = success ? (TNumber)(object)decimalResult : default;
                return success;
            }
            if (typeof(TNumber) == typeof(double))
            {
                var success = double.TryParse(value, out var doubleResult);
                result = success ? (TNumber)(object)doubleResult : default;
                return success;
            }
            if (typeof(TNumber) == typeof(float))
            {
                var success = float.TryParse(value, out var floatResult);
                result = success ? (TNumber)(object)floatResult : default;
                return success;
            }
            if (typeof(TNumber) == typeof(long))
            {
                var success = long.TryParse(value, out var longResult);
                result = success ? (TNumber)(object)longResult : default;
                return success;
            }
            
            result = default;
            return false;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}
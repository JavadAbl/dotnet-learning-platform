using Contracts.Dto.Request;
using System.Linq.Expressions;

namespace Contracts.Extensions;


public static class QueryableExtensions
{
    public static IQueryable<T> ApplyGetManyQuery<T>(
        this IQueryable<T> query,
        GetManyQuery? criteria,
        IEnumerable<string>? searchableFields = null)
    {
        if (criteria is null) return query;

        // 1. Pagination
        var safePage = Math.Max(criteria.Page ?? 1, 1);
        var safePageSize = Math.Min(criteria.PageSize ?? 100, 100);
        query = query.Skip((safePage - 1) * safePageSize).Take(safePageSize);

        // 2. Dynamic Sorting via Expression Trees
        if (!string.IsNullOrEmpty(criteria.SortBy))
        {
            query = ApplyDynamicSorting(query, criteria.SortBy, criteria.SortOrder ?? "asc");
        }

        // 3. Dynamic Search via Expression Trees
        if (!string.IsNullOrEmpty(criteria.Search) && searchableFields != null && searchableFields.Any())
        {
            query = ApplyDynamicSearch(query, criteria.Search, searchableFields);
        }

        return query;
    }

    private static IQueryable<T> ApplyDynamicSorting<T>(IQueryable<T> query, string sortBy, string sortOrder)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortBy);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda })!;
    }

    private static IQueryable<T> ApplyDynamicSearch<T>(IQueryable<T> query, string search, IEnumerable<string> searchableFields)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? combinedCondition = null;
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        foreach (var field in searchableFields)
        {
            var property = Expression.Property(parameter, field);
            var searchValue = Expression.Constant(search, typeof(string));
            var condition = Expression.Call(property, containsMethod!, searchValue);

            combinedCondition = combinedCondition == null
                ? condition
                : Expression.OrElse(combinedCondition, condition);
        }

        if (combinedCondition != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedCondition, parameter);
            query = query.Where(lambda);
        }

        return query;
    }
}
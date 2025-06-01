using System.Linq.Expressions;

namespace AspNetCoreSharedKernel;


public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> queryable,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? queryable.Where(predicate) : queryable;
    }

    public static (Task<List<TEntity>>, Task<long>) Paginate<TEntity, TCursor>(
        this IQueryable<TEntity> queryable,
        TCursor pageCursor,
        Func<TEntity, TCursor> field,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        where TEntity : EntityBase<TCursor>
        where TCursor : class, IEquatable<TCursor>, IComparable<TCursor>
    {
        return (queryable
            .SkipWhile(p => field(p).CompareTo(pageCursor) <= 0)
            .Take(pageSize)
            .ToListAsync(cancellationToken), queryable.LongCountAsync(cancellationToken));
    }
}

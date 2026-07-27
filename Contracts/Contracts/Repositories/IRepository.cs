using Contracts.Dto.Response;
using System.Linq.Expressions;


namespace Contracts.Contracts.Repositories;

public interface IRepository<T> where T : class
{
    // ── Read ──────────────────────────────────────────────
    Task<T?> GetByIdAsync(object id);
    Task<GetManyResponse<TDto>> FindMany<TDto>(IQueryable<TDto> query);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    // ── Create ────────────────────────────────────────────
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    // ── Update ────────────────────────────────────────────
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    // ── Delete ────────────────────────────────────────────
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    // ── Query helpers ─────────────────────────────────────
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

    // ── Persistence ───────────────────────────────────────
    Task<int> SaveChangesAsync();

    IQueryable<T> GetQueryable();

}

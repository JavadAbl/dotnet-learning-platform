using Contracts.Dto.Request;
using Contracts.Dto.Response;
using System.Linq.Expressions;


namespace Contracts.Infrastructure.Database.Repositories;

public interface IRepository<TEntity, TDto> where TEntity : class
{
    // ── Read ──────────────────────────────────────────────
    Task<TEntity?> GetByIdAsync(object id);
    Task<GetManyResponse<TDto>> FindMany(GetManyQuery? predicate, string[] searchableFields);
    Task<TDto> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    // ── Create ────────────────────────────────────────────
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    // ── Update ────────────────────────────────────────────
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    // ── Delete ────────────────────────────────────────────
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    // ── Query helpers ─────────────────────────────────────
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    // ── Persistence ───────────────────────────────────────
    Task<int> SaveChangesAsync();

    IQueryable<TEntity> GetQueryable();

}

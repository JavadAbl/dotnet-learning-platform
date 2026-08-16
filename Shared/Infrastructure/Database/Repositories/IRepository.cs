using Shared.Dto.Request;
using Shared.Dto.Response;
using System.Linq.Expressions;


namespace Shared.Infrastructure.Database.Repositories;

public interface IRepository<TEntity, TDto> where TEntity : class
{
    // ── Read ──────────────────────────────────────────────
    Task<TEntity?> GetByIdAsync(object id);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindMany(GetManyQuery? predicate, string[] searchableFields);

    // ── Dto ──────────────────────────────────────────────
    Task<TDto?> FirstDtoOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<TDto> FirstDto(Expression<Func<TEntity, bool>> predicate);
    Task CheckDuplicate(Expression<Func<TEntity, bool>> predicate);
    Task<GetManyResponse<TDto>> FindManyDto(GetManyQuery? predicate, string[] searchableFields);


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

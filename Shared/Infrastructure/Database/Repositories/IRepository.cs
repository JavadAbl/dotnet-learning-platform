using Shared.Dto.Request;
using Shared.Dto.Response;
using System.Linq.Expressions;


namespace Shared.Infrastructure.Database.Repositories;

public interface IRepository<TEntity, TDto, TCreate, TUpdate> where TEntity : class
{
    // ── Read ──────────────────────────────────────────────
    Task<TEntity?> GetById(object id);
    Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> First(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindMany(GetManyQuery? predicate, string[] searchableFields);

    // ── Dto ──────────────────────────────────────────────
    Task<TDto?> FirstDtoOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<TDto> FirstDto(Expression<Func<TEntity, bool>> predicate);
    Task CheckDuplicate(Expression<Func<TEntity, bool>> predicate);
    Task<GetManyResponse<TDto>> FindDtoMany(GetManyQuery? predicate, string[] searchableFields);


    // ── Create ────────────────────────────────────────────
    Task<TEntity> Create(TCreate dto);
    Task AddRange(IEnumerable<TEntity> entities);

    // ── Update ────────────────────────────────────────────
    Task<TEntity> UpdatePartial(object id, TEntity entity, TUpdate dto);
    void UpdateRange(IEnumerable<TEntity> entities);

    // ── Delete ────────────────────────────────────────────
    Task Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    // ── Query helpers ─────────────────────────────────────
    Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
    Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null);

    // ── Persistence ───────────────────────────────────────
    Task<int> SaveChanges();

    IQueryable<TEntity> GetQueryable();

}

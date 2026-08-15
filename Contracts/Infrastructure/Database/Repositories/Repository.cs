using Contracts.Dto.Request;
using Contracts.Dto.Response;
using Contracts.Exceptions;
using Contracts.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Contracts.Infrastructure.Database.Repositories;

public class Repository<TEntity, TDto> : IRepository<TEntity, TDto> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly Expression<Func<TEntity, TDto>> _selector;

    public Repository(DbContext context, Expression<Func<TEntity, TDto>> selector)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _dbSet = _context.Set<TEntity>();
    }

    // ── Read ──────────────────────────────────────────────

    public virtual async Task<TEntity?> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    /* public virtual async Task<GetManyResponse<TDto>> FindMany<TDto>(
      IQueryable<TDto> query)
     {
         var itemsTask = query.ToListAsync();
         var countTask = query.CountAsync();
         await Task.WhenAll(itemsTask, countTask);

         return new GetManyResponse<TDto>(countTask.Result, itemsTask.Result);
     }*/


    public virtual async Task<GetManyResponse<TDto>> FindMany(
      GetManyQuery? predicate,
      string[] searchableFields)
    {
        var query = _dbSet.AsQueryable();

        // Filter FIRST (on T, before projection)
        if (predicate != null)
            query = query.ApplyGetManyQuery(predicate, searchableFields);

        // Project to TDto
        if (_selector == null)
            throw new ArgumentNullException(nameof(_selector),
                "A selector is required to project T to TDto.");

        IQueryable<TDto> dtoQuery = query.Select(_selector);

        var itemsTask = dtoQuery.ToListAsync();
        var countTask = _dbSet.CountAsync();
        await Task.WhenAll(itemsTask, countTask);

        return new GetManyResponse<TDto>(countTask.Result, itemsTask.Result);
    }

    public virtual async Task<TDto> FirstOrDefaultAsync(
     Expression<Func<TEntity, bool>> predicate)

    {
        var dto = await _dbSet
            .Where(predicate)
            .Select(_selector)
            .FirstOrDefaultAsync();

        if (dto == null)
            throw new NotFoundException();

        return dto;
    }

    // ── Create ────────────────────────────────────────────

    public virtual async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        => await _dbSet.AddRangeAsync(entities);

    // ── Update ────────────────────────────────────────────

    public virtual void Update(TEntity entity)
        => _dbSet.Update(entity);

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
        => _dbSet.UpdateRange(entities);

    // ── Delete ────────────────────────────────────────────

    public virtual void Remove(TEntity entity)
        => _dbSet.Remove(entity);

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
        => _dbSet.RemoveRange(entities);

    // ── Query helpers ─────────────────────────────────────

    public virtual async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null)
        => predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);

    // ── Persistence ───────────────────────────────────────

    public virtual async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable<TEntity>();


}
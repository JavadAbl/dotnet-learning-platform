using Contracts.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Contracts.Infrastructure.Database.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    // ── Read ──────────────────────────────────────────────

    public virtual async Task<T?> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);

    // ── Create ────────────────────────────────────────────

    public virtual async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbSet.AddRangeAsync(entities);

    // ── Update ────────────────────────────────────────────

    public virtual void Update(T entity)
        => _dbSet.Update(entity);

    public virtual void UpdateRange(IEnumerable<T> entities)
        => _dbSet.UpdateRange(entities);

    // ── Delete ────────────────────────────────────────────

    public virtual void Remove(T entity)
        => _dbSet.Remove(entity);

    public virtual void RemoveRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);

    // ── Query helpers ─────────────────────────────────────

    public virtual async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public virtual async Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null)
        => predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);

    // ── Persistence ───────────────────────────────────────

    public virtual async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
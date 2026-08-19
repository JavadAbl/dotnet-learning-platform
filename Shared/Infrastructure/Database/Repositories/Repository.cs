using Microsoft.EntityFrameworkCore;
using Shared.Dto.Request;
using Shared.Dto.Response;
using Shared.Exceptions;
using Shared.Infrastructure.Database.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Infrastructure.Database.Repositories;

public class Repository<TEntity, TDto, TCreate, TUpdate> : IRepository<TEntity, TDto, TCreate, TUpdate> where TEntity : class
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

    public virtual async Task<TEntity?> GetById(object id)
        => await _dbSet.FindAsync(id);

    /* public virtual async Task<GetManyResponse<TDto>> FindMany<TDto>(
      IQueryable<TDto> query)
     {
         var itemsTask = query.ToListAsync();
         var countTask = query.CountAsync();
         await Task.WhenAll(itemsTask, countTask);

         return new GetManyResponse<TDto>(countTask.Result, itemsTask.Result);
     }*/


    public async Task<IEnumerable<TEntity>> FindMany(GetManyQuery? predicate, string[] searchableFields)
    {
        var query = _dbSet.AsQueryable();

        if (predicate != null)
            query = query.ApplyGetManyQuery(predicate, searchableFields);

        return await query.ToListAsync();
    }

    public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await _dbSet
           .Where(predicate)
           .FirstOrDefaultAsync();

        return entity;
    }

    public virtual async Task<TEntity> First(
      Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await _dbSet
            .Where(predicate)
            .FirstOrDefaultAsync();

        if (entity is null)
            throw new NotFoundException();

        return entity;
    }

    // ── Dto ────────────────────────────────────────────

    public virtual async Task<GetManyResponse<TDto>> FindDtoMany(
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

    public virtual async Task<TDto?> FirstDtoOrDefault(
     Expression<Func<TEntity, bool>> predicate)

    {
        var dto = await _dbSet
            .Where(predicate)
            .Select(_selector)
            .FirstOrDefaultAsync();

        return dto;
    }

    public virtual async Task<TDto> FirstDto(
        Expression<Func<TEntity, bool>> predicate)
    {
        var dto = await _dbSet
            .Where(predicate)
            .Select(_selector)
            .FirstOrDefaultAsync();

        if (dto is null)
            throw new NotFoundException();

        return dto;
    }

    public virtual async Task CheckDuplicate(
           Expression<Func<TEntity, bool>> predicate)
    {
        var dto = await _dbSet
            .Where(predicate)
            .FirstOrDefaultAsync();

        if (dto is not null)
            throw new ConflictException();

    }


    // ── Create ────────────────────────────────────────────


    public async Task<TEntity> Create(TEntity entity)
    {
        _dbSet.Add(entity);
        await SaveChanges();
        return entity;
    }

    public virtual async Task<TEntity> CreateFromDto(TCreate dto)
    {
        // --- Recommended approach if you have a mapper injected (e.g., AutoMapper) ---
        // var entity = _mapper.Map<TEntity>(dto);

        // --- Fallback approach using Reflection ---
        // nonPublic: true allows instantiation even if the entity has a private parameterless constructor
        var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), nonPublic: true)!;
        MapProperties(dto, entity);

        await _dbSet.AddAsync(entity);
        await SaveChanges();

        return entity;
    }

    public virtual async Task AddRange(IEnumerable<TEntity> entities)
        => await _dbSet.AddRangeAsync(entities);

    // ── Update ────────────────────────────────────────────

    public virtual async Task<TEntity> UpdatePartial(object id, TEntity entity, TUpdate dto)
    {
        if (entity is null)
            throw new NotFoundException();

        // --- Recommended approach if you have a mapper injected ---
        // _mapper.Map(dto, entity); // Maps dto onto the existing tracked entity

        // --- Fallback approach using Reflection ---
        MapProperties(dto, entity);

        // Because the entity was fetched via FindAsync, it is tracked by EF Core.
        // EF Core's ChangeTracker automatically detects which properties actually changed 
        // and will only generate SQL UPDATE statements for those specific columns (True Partial Update).
        await SaveChanges();

        return entity;
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
        => _dbSet.UpdateRange(entities);

    // ── Reflection Mapping Helper ─────────────────────────

    protected virtual void MapProperties<TSource, TTarget>(TSource source, TTarget target)
    {
        if (source == null || target == null) return;

        var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var targetProps = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var sourceProp in sourceProps)
        {
            if (!sourceProp.CanRead) continue;

            var value = sourceProp.GetValue(source);

            // Optional: Uncomment the line below if you want to ignore null values 
            // during partial updates to prevent overwriting existing data with nulls.
            // if (value == null) continue;

            var targetProp = targetProps.FirstOrDefault(p => p.Name == sourceProp.Name && p.CanWrite);

            if (targetProp != null)
            {
                try
                {
                    if (value == null || targetProp.PropertyType.IsAssignableFrom(value.GetType()))
                    {
                        targetProp.SetValue(target, value);
                    }
                    else
                    {
                        // Handle nullable underlying types (e.g., int? -> int) and basic conversions
                        var targetType = Nullable.GetUnderlyingType(targetProp.PropertyType) ?? targetProp.PropertyType;
                        var convertedValue = Convert.ChangeType(value, targetType);
                        targetProp.SetValue(target, convertedValue);
                    }
                }
                catch
                {
                    // Silently ignore properties that cannot be mapped/converted
                }
            }
        }
    }

    // ── Delete ────────────────────────────────────────────

    public virtual async Task Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
        await SaveChanges();
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
        => _dbSet.RemoveRange(entities);

    // ── Query helpers ─────────────────────────────────────

    public virtual async Task<bool> Any(
        Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public virtual async Task<int> Count(
        Expression<Func<TEntity, bool>>? predicate = null)
        => predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);

    // ── Persistence ───────────────────────────────────────

    public virtual async Task<int> SaveChanges()
        => await _context.SaveChangesAsync();

    public IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable<TEntity>();


}
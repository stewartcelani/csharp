using System.Linq.Expressions;
using CityInfo.API.Data;
using CityInfo.API.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories.Common;

public abstract class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }


    public virtual async Task<TEntity?> GetAsync(TKey id, IEnumerable<string>? includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        
        if (includeProperties is not null)
            foreach (var property in includeProperties)
                query = query.Include(property.Trim());
        
        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        IEnumerable<string>? includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate is not null) query = query.Where(predicate);

        if (includeProperties is not null)
            foreach (var property in includeProperties)
                query = query.Include(property.Trim());

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> CreateAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
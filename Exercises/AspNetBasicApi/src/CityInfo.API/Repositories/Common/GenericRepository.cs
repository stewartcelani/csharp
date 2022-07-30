using System.Linq.Expressions;
using CityInfo.API.Data;
using CityInfo.API.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories.Common;

public abstract class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected GenericRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<TEntity>();
    }


    public virtual async Task<TEntity?> GetAsync(TKey id, IEnumerable<string?> includeProperties = null)
    {
        IQueryable<TEntity> query = DbSet;
        
        if (includeProperties is not null)
            foreach (var property in includeProperties)
                query = query.Include(property.Trim());
        
        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        IEnumerable<string>? includeProperties = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (predicate is not null) query = query.Where(predicate);

        if (includeProperties is not null)
            foreach (var property in includeProperties)
                query = query.Include(property.Trim());

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(TKey id) => await DbSet.AnyAsync(x => x.Id.Equals(id));

    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        DbSet.Add(entity);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> CreateAsync(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        var entity = await GetAsync(id);
        if (entity is null) return false;
        DbSet.Remove(entity);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(IEnumerable<TKey> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0) return false;
        var entities = new List<TEntity>();
        foreach (var id in idList)
        {
            var entity = await GetAsync(id);
            if (entity is null) continue;
            entities.Add(entity);
        }
        DbSet.RemoveRange(entities);
        return await DbContext.SaveChangesAsync() > 0;
    }
}
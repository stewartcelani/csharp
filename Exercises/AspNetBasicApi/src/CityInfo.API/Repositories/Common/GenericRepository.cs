using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CityInfo.API.Data;
using CityInfo.API.Domain.Entities.Common;
using CityInfo.API.Domain.Filters;
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
        IEnumerable<string>? includeProperties = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        PaginationFilter? paginationFilter = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (predicate is not null) query = query.Where(predicate);

        if (includeProperties is not null)
            foreach (var property in includeProperties)
                query = query.Include(property.Trim());

        if (orderBy is not null) query = orderBy(query);

        if (paginationFilter is not null)
        {
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            query = query.Skip(skip).Take(paginationFilter.PageSize);
        }
        
        query = query.AsNoTracking();
        
        /*if (orderBy is not null) return await orderBy(query).ToListAsync();*/

        return await query.ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await DbSet.AsNoTracking().AnyAsync(x => x.Id.Equals(id));
    }

    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        DbContext.ChangeTracker.Clear();
        DbSet.Add(entity);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> CreateAsync(IEnumerable<TEntity> entities)
    {
        DbContext.ChangeTracker.Clear();
        DbSet.AddRange(entities);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        DbContext.ChangeTracker.Clear();
        DbSet.Update(entity);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
    {
        DbContext.ChangeTracker.Clear();
        DbSet.UpdateRange(entities);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        DbContext.ChangeTracker.Clear();
        var entity = await GetAsync(id);
        if (entity is null) return false;
        DbSet.Remove(entity);
        return await DbContext.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(IEnumerable<TKey> ids)
    {
        DbContext.ChangeTracker.Clear();
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
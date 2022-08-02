using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CityInfo.API.Domain.Entities.Common;
using CityInfo.API.Domain.Filters;

namespace CityInfo.API.Repositories.Common;

public interface IRepository<TEntity, in TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task<bool> ExistsAsync(TKey id);
    Task<TEntity?> GetAsync(TKey id, IEnumerable<string?> includeProperties = null);

    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        IEnumerable<string>? includeProperties = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        PaginationFilter? paginationFilter = null);

    Task<bool> CreateAsync(TEntity entity);
    Task<bool> CreateAsync(IEnumerable<TEntity> entities);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> UpdateAsync(IEnumerable<TEntity> entities);
    Task<bool> DeleteAsync(TKey id);
    Task<bool> DeleteAsync(IEnumerable<TKey> ids);
}
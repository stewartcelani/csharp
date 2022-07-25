using System.Linq.Expressions;
using CityInfo.API.Data;
using CityInfo.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<CityEntity> _dbSet;

    public CityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<CityEntity>();
    }

    public async Task<CityEntity?> GetAsync(Guid id) => await _dbSet.Include(x => x.PointsOfInterest)
        .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<CityEntity>> GetAsync(Expression<Func<CityEntity, bool>>? predicate = null)
    {
        IQueryable<CityEntity> query = _dbSet.Include(x => x.PointsOfInterest).AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> CreateAsync(CityEntity cityEntity)
    {
        _dbSet.Add(cityEntity);
        var rowsUpdated = await _dbContext.SaveChangesAsync();
        return rowsUpdated > 0;
    }

    public async Task<bool> UpdateAsync(CityEntity cityEntity)
    {
        _dbSet.Update(cityEntity);
        var rowsUpdated = await _dbContext.SaveChangesAsync();
        return rowsUpdated > 0;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var cityEntity = await GetAsync(id);
        if (cityEntity is null) return true;
        _dbSet.Remove(cityEntity);
        var rowsDeleted = await _dbContext.SaveChangesAsync();
        return rowsDeleted > 0;
    }
}
using System.Linq.Expressions;
using CityInfo.API.Data;
using CityInfo.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class CityRepository : ICityRepository
{
    private readonly DbSet<CityEntity> _dbSet;

    public CityRepository(ApplicationDbContext dbContext)
    {
        _dbSet = dbContext.Set<CityEntity>();
    }

    public async Task<CityEntity?> GetAsync(Guid id) => await _dbSet.AsNoTracking().Include(x => x.PointsOfInterest)
        .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<CityEntity>> GetAsync(Expression<Func<CityEntity, bool>>? predicate = null)
    {
        IQueryable<CityEntity> query = _dbSet.AsNoTracking().Include(x => x.PointsOfInterest).AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }
}
using CityInfo.API.Data;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class PointOfInterestRepository : IPointOfInterestRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<PointOfInterestEntity> _dbSet;

    public PointOfInterestRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<PointOfInterestEntity>();
    }

    public async Task<PointOfInterestEntity?> GetAsync(Guid id) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<bool> CreateAsync(PointOfInterestEntity pointOfInterestEntity)
    {
        _dbSet.Add(pointOfInterestEntity);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAsync(PointOfInterestEntity pointOfInterestEntity)
    {
        _dbSet.Update(pointOfInterestEntity);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var pointOfInterestEntity = await GetAsync(id);
        if (pointOfInterestEntity is null) return true;
        _dbSet.Remove(pointOfInterestEntity);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}
using CityInfo.API.Data;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Repositories.Common;

namespace CityInfo.API.Repositories;

public class CityRepository : GenericRepository<CityEntity, Guid>, ICityRepository
{
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<bool> UpdateAsync(CityEntity entity)
    {
        // Points of interest have their own repository with an update method
        // This stops all points of interest from being set to empty list and deleted
        DbContext.Entry(entity).Property(x => x.PointsOfInterest).IsModified = false;
        return base.UpdateAsync(entity);
    }

    public override Task<bool> UpdateAsync(IEnumerable<CityEntity> entities)
    {
        var cityEntities = entities.ToList();
        foreach (var entity in cityEntities)
        {
            DbContext.Entry(entity).Property(x => x.PointsOfInterest).IsModified = false;
        }
        return base.UpdateAsync(cityEntities);
    }
}
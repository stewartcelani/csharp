using CityInfo.API.Data;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Repositories.Common;

namespace CityInfo.API.Repositories;

public class PointOfInterestRepository : GenericRepository<PointOfInterestEntity, Guid>, IPointOfInterestRepository
{
    public PointOfInterestRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
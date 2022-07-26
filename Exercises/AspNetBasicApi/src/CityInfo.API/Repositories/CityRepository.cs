using CityInfo.API.Data;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Repositories.Common;

namespace CityInfo.API.Repositories;

public class CityRepository : GenericRepository<CityEntity, Guid>, ICityRepository
{
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
using CityInfo.API.Domain.Entities;
using CityInfo.API.Repositories.Common;

namespace CityInfo.API.Repositories;

public interface ICityRepository : IRepository<CityEntity, Guid>
{
}
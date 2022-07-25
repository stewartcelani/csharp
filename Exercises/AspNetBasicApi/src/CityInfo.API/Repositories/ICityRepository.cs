using System.Linq.Expressions;
using CityInfo.API.Domain.Entities;

namespace CityInfo.API.Repositories;

public interface ICityRepository
{
    Task<CityEntity?> GetAsync(Guid id);
    Task<IEnumerable<CityEntity>> GetAsync(Expression<Func<CityEntity, bool>>? predicate = null);
}
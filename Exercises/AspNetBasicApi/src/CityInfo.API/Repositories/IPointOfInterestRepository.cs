using CityInfo.API.Domain.Entities;

namespace CityInfo.API.Repositories;

public interface IPointOfInterestRepository
{
    Task<PointOfInterestEntity?> GetAsync(Guid id);
    Task<bool> CreateAsync(PointOfInterestEntity pointOfInterestEntity);
    Task<bool> UpdateAsync(PointOfInterestEntity pointOfInterestEntity);
    Task<bool> DeleteAsync(Guid id);
}
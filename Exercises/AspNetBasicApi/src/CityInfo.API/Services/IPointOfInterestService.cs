using CityInfo.API.Domain;

namespace CityInfo.API.Services;

public interface IPointOfInterestService
{
    Task<PointOfInterest?> GetByIdAsync(Guid id);
    Task<IEnumerable<PointOfInterest>> GetAllAsync(Guid cityId);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> CreateAsync(Guid cityId, PointOfInterest pointOfInterest);
    Task<bool> UpdateAsync(Guid cityId, PointOfInterest pointOfInterest);
    Task<bool> DeleteAsync(Guid pointOfInterestId);
}
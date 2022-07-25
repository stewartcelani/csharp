using CityInfo.API.Domain;

namespace CityInfo.API.Services;

public interface ICityService
{
    Task<City?> GetByIdAsync(Guid id);
    Task<IEnumerable<City>> GetAllAsync();
    Task<bool> CreatePointOfInterestAsync(City city, PointOfInterest pointOfInterest);
    Task<bool> UpdatePointOfInterestAsync(City city, PointOfInterest pointOfInterest);
    Task<bool> DeletePointOfInterestAsync(Guid pointOfInterestId);
}
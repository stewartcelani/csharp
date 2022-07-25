using CityInfo.API.Domain;

namespace CityInfo.API.Services;

public interface IPointOfInterestService
{
    Task<bool> CreateAsync(City city, PointOfInterest pointOfInterest);
    Task<bool> UpdateAsync(City city, PointOfInterest pointOfInterest);
    Task<bool> DeleteAsync(Guid pointOfInterestId);
}
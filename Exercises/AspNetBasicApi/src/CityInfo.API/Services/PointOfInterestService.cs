using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;

namespace CityInfo.API.Services;

public class PointOfInterestService : IPointOfInterestService
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;

    public PointOfInterestService(IPointOfInterestRepository pointOfInterestRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository ??
                                     throw new NullReferenceException(nameof(pointOfInterestRepository));
    }

    public async Task<bool> CreateAsync(City city, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city);
        var created = await _pointOfInterestRepository.CreateAsync(pointOfInterestEntity);
        return created;
    }

    public async Task<bool> UpdateAsync(City city, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city);
        var updated = await _pointOfInterestRepository.UpdateAsync(pointOfInterestEntity);
        return updated;
    }

    public async Task<bool> DeleteAsync(City city, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city);
        var deleted = await _pointOfInterestRepository.DeleteAsync(pointOfInterestEntity);
        return deleted;
    }
}
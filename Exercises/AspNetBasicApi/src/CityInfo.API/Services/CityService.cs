using System.Text.Json;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Data;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;

namespace CityInfo.API.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IPointOfInterestRepository _pointOfInterestRepository;

    public CityService(ICityRepository cityRepository, IPointOfInterestRepository pointOfInterestRepository)
    {
        _cityRepository = cityRepository ?? throw new NullReferenceException(nameof(cityRepository));
        _pointOfInterestRepository = pointOfInterestRepository ??
                                     throw new NullReferenceException(nameof(pointOfInterestRepository));
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        var cityEntity = await _cityRepository.GetAsync(id);
        return cityEntity?.ToCity();
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        var cityEntities = await _cityRepository.GetAsync();
        return cityEntities.Select(x => x.ToCity());
    }
    
    public async Task<bool> CreatePointOfInterestAsync(City city, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city);
        var created = await _pointOfInterestRepository.CreateAsync(pointOfInterestEntity);
        return created;
    }
    
    public async Task<bool> UpdatePointOfInterestAsync(City city, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city);
        var updated = await _pointOfInterestRepository.UpdateAsync(pointOfInterestEntity);
        return updated;
    }
    
    public async Task<bool> DeletePointOfInterestAsync(Guid pointOfInterestId)
    {
        var deleted = await _pointOfInterestRepository.DeleteAsync(pointOfInterestId);
        return deleted;
    }
}
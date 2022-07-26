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

    public async Task<PointOfInterest?> GetByIdAsync(Guid id)
    {
        var pointOfInterestEntity = await _pointOfInterestRepository.GetAsync(id);
        return pointOfInterestEntity?.ToPointOfInterest();
    }

    public async Task<IEnumerable<PointOfInterest>> GetAllAsync(Guid cityId)
    {
        var pointOfInterestEntities =
            await _pointOfInterestRepository.GetAsync(x => x.CityId == cityId);
        return pointOfInterestEntities.Select(x => x.ToPointOfInterest());
    }

    public async Task<bool> ExistsAsync(Guid id) => await _pointOfInterestRepository.ExistsAsync(id);

    public async Task<bool> CreateAsync(Guid cityId, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(cityId);
        var created = await _pointOfInterestRepository.CreateAsync(pointOfInterestEntity);
        return created;
    }

    public async Task<bool> UpdateAsync(Guid cityId, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(cityId);
        var updated = await _pointOfInterestRepository.UpdateAsync(pointOfInterestEntity);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid cityId, PointOfInterest pointOfInterest)
    {
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(cityId);
        var deleted = await _pointOfInterestRepository.DeleteAsync(pointOfInterestEntity);
        return deleted;
    }
}
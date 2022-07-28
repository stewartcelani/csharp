using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using CityInfo.API.Validators.Helpers;
using FluentValidation;

namespace CityInfo.API.Services;

public class PointOfInterestService : IPointOfInterestService
{
    private readonly IPointOfInterestRepository _pointOfInterestRepository;
    private readonly ICityRepository _cityRepository;

    public PointOfInterestService(IPointOfInterestRepository pointOfInterestRepository, ICityRepository cityRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository ??
                                     throw new NullReferenceException(nameof(pointOfInterestRepository));
        _cityRepository = cityRepository ?? throw new NullReferenceException(nameof(cityRepository));
    }

    public async Task<bool> ExistsAsync(Guid id) => await _pointOfInterestRepository.ExistsAsync(id);
    
    public async Task<PointOfInterest?> GetByIdAsync(Guid id)
    {
        var pointOfInterestEntity = await _pointOfInterestRepository.GetAsync(id);
        return pointOfInterestEntity?.ToPointOfInterest();
    }

    public async Task<IEnumerable<PointOfInterest>> GetAllAsync(Guid cityId)
    {
        await CheckAndThrowValidationExceptionIfCityDoesNotExist(cityId);
        
        var pointOfInterestEntities =
            await _pointOfInterestRepository.GetAsync(x => x.CityId == cityId);
        return pointOfInterestEntities.Select(x => x.ToPointOfInterest());
    }

    public async Task<bool> CreateAsync(Guid cityId, PointOfInterest pointOfInterest)
    {
        await CheckAndThrowValidationExceptionIfCityDoesNotExist(cityId);
        
        if (await _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id))
        {
            var message = $"A point of interest with id {pointOfInterest.Id} already exists";
            throw new ValidationException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(cityId);
        var created = await _pointOfInterestRepository.CreateAsync(pointOfInterestEntity);
        return created;
    }

    public async Task<bool> UpdateAsync(Guid cityId, PointOfInterest pointOfInterest)
    {
        await CheckAndThrowValidationExceptionIfCityDoesNotExist(cityId);
        
        if (!await _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id))
        {
            var message = $"Can not update point of interest with id {pointOfInterest.Id} as it does not exist";
            throw new ValidationException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(cityId);
        var updated = await _pointOfInterestRepository.UpdateAsync(pointOfInterestEntity);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid cityId, PointOfInterest pointOfInterest)
    {
        await CheckAndThrowValidationExceptionIfCityDoesNotExist(cityId);
        
        if (!await _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id)) return true;

        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(cityId);
        var deleted = await _pointOfInterestRepository.DeleteAsync(pointOfInterestEntity);
        return deleted;
    }
    
    private async Task CheckAndThrowValidationExceptionIfCityDoesNotExist(Guid cityId)
    {
        if (!await _cityRepository.ExistsAsync(cityId))
        {
            var message = $"City with id {cityId} does not exist";
            throw new ValidationException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }
    }
}
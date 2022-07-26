using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using CityInfo.API.Validators.Helpers;
using FluentValidation;

namespace CityInfo.API.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    private readonly string[] _defaultIncludeProperties = { nameof(CityEntity.PointsOfInterest) };

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository ?? throw new NullReferenceException(nameof(cityRepository));
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        var cityEntity = await _cityRepository.GetAsync(id, _defaultIncludeProperties);
        return cityEntity?.ToCity();
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        var cityEntities =
            await _cityRepository.GetAsync(includeProperties: _defaultIncludeProperties);
        return cityEntities.Select(x => x.ToCity());
    }

    public async Task<bool> ExistsAsync(Guid id) => await _cityRepository.ExistsAsync(id);

    public async Task<bool> CreateAsync(City city)
    {
        var existingCity = await _cityRepository.GetAsync(city.Id);
        if (existingCity is not null)
        {
            var message = $"A city with id {city.Id} already exists";
            throw new ValidationException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        var cityEntity = city.ToCityEntity();
        var created = await _cityRepository.CreateAsync(cityEntity);
        return created;
    }

    public async Task<bool> UpdateAsync(City city)
    {
        var cityEntity = city.ToCityEntity();
        var updated = await _cityRepository.UpdateAsync(cityEntity);
        return updated;
    }

    public async Task<bool> DeleteAsync(City city)
    {
        var cityEntity = city.ToCityEntity();
        var deleted = await _cityRepository.DeleteAsync(cityEntity);
        return deleted;
    }
}
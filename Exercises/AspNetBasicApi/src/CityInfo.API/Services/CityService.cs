using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Domain.Filters;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using CityInfo.API.Validators.Helpers;
using ValidationException = FluentValidation.ValidationException;

namespace CityInfo.API.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    private readonly string[] _defaultIncludeProperties = { nameof(CityEntity.PointsOfInterest) };

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository ?? throw new NullReferenceException(nameof(cityRepository));
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _cityRepository.ExistsAsync(id);
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        var cityEntity = await _cityRepository.GetAsync(id, _defaultIncludeProperties);
        return cityEntity?.ToCity();
    }

    public async Task<IEnumerable<City>> GetAsync(GetCitiesFilter getCitiesFilter, PaginationFilter paginationFilter)
    {
        Expression<Func<CityEntity, bool>>? predicate = null;
        if (getCitiesFilter?.Name is not null)
        {
            predicate = entity => entity.Name.Trim().ToLower().Contains(getCitiesFilter.Name.Trim().ToLower());
        }
        
        var cityEntities =
            await _cityRepository.GetAsync(predicate, _defaultIncludeProperties, q => q.OrderBy(x => x.Name), paginationFilter);
        return cityEntities.Select(x => x.ToCity());
    }
    
    public async Task<IEnumerable<City>> GetAsync()
    {
        return await GetAsync(new GetCitiesFilter(), new PaginationFilter());
    }
    
    public async Task<IEnumerable<City>> GetAsync(string name)
    {
        name = name.Trim().ToLower();
        var cityEntities =
            await _cityRepository.GetAsync(x => x.Name.ToLower().Contains(name), _defaultIncludeProperties, q => q.OrderBy(x => x.Name));
        return cityEntities.Select(x => x.ToCity());
    }

    public async Task<bool> CreateAsync(City city)
    {
        if (await _cityRepository.ExistsAsync(city.Id))
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
        if (!await _cityRepository.ExistsAsync(city.Id))
        {
            var message = $"Can not update city with id {city.Id} as it does not exist";
            throw new ValidationException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        var cityEntity = city.ToCityEntity();
        var updated = await _cityRepository.UpdateAsync(cityEntity);
        return updated;
    }

    public async Task<bool> DeleteAsync(Guid cityId)
    {
        if (!await _cityRepository.ExistsAsync(cityId)) return true;
        var deleted = await _cityRepository.DeleteAsync(cityId);
        return deleted;
    }
}
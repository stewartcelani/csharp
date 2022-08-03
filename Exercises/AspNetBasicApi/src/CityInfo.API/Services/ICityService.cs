using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Filters;

namespace CityInfo.API.Services;

public interface ICityService
{
    Task<City?> GetByIdAsync(Guid id);
    Task<IEnumerable<City>> GetAsync();
    Task<IEnumerable<City>> GetAsync(GetCitiesFilter getCitiesFilter, PaginationFilter paginationFilter);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> CreateAsync(City city);
    Task<bool> UpdateAsync(City city);
    Task<bool> DeleteAsync(Guid cityId);
}
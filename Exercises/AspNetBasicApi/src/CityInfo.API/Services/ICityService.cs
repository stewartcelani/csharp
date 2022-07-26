using CityInfo.API.Domain;

namespace CityInfo.API.Services;

public interface ICityService
{
    Task<City?> GetByIdAsync(Guid id);
    Task<IEnumerable<City>> GetAllAsync();
    Task<bool> ExistsAsync(Guid id);
    Task<bool> CreateAsync(City city);
    Task<bool> UpdateAsync(City city);
    Task<bool> DeleteAsync(City city);
}
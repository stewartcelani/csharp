using System.Text.Json;
using CityInfo.API.Attributes;
using CityInfo.API.Contracts.v1;
using CityInfo.API.Contracts.v1.Requests;
using CityInfo.API.Contracts.v1.Requests.Queries;
using CityInfo.API.Contracts.v1.Responses.Helpers;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using CityInfo.API.Validators.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers.v1;

[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IUriService _uriService;

    public CityController(ICityService cityService, IUriService uriService)
    {
        _cityService = cityService ?? throw new NullReferenceException(nameof(cityService));
        _uriService = uriService ?? throw new NullReferenceException(nameof(uriService));
    }

    [Cached(60)]
    [HttpGet(ApiRoutesV1.Cities.GetAll.Url)]
    public async Task<IActionResult> GetCities([FromQuery] GetCitiesQuery query, [FromQuery] PaginationQuery paginationQuery)
    {
        var getCitiesFilter = query.ToGetCitiesFilter();
        var paginationFilter = paginationQuery.ToPaginationFilter();

        var cities = await _cityService.GetAsync(getCitiesFilter, paginationFilter);

        var citiesResponse = cities.Select(x => x.ToExtendedCityResponse()).ToList();

        var pagedResponse =
            PagedResponseHelper.CreatePaginatedCityResponse(_uriService, paginationFilter, citiesResponse);

        return Ok(pagedResponse);
    }

    [Cached(60)]
    [HttpGet(ApiRoutesV1.Cities.Get.Url)]
    public async Task<IActionResult> GetCity([FromRoute] Guid cityId)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var cityResponse = city.ToExtendedCityResponse();

        return Ok(cityResponse);
    }

    [HttpPost(ApiRoutesV1.Cities.Create.Url)]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
    {
        var city = request.ToCity();

        var created = await _cityService.CreateAsync(city);

        if (!created)
        {
            var message = $"Error creating city: {JsonSerializer.Serialize(city)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        var cityResponse = city.ToCityResponse();

        return CreatedAtAction(nameof(GetCity),
            new { cityId = cityResponse.Id }, cityResponse);
    }
    
    [HttpPut(ApiRoutesV1.Cities.Update.Url)]
    public async Task<IActionResult> UpdateCity([FromMultiSource] UpdateCityRequest request)
    {
        if (!await _cityService.ExistsAsync(request.Id)) return NotFound();

        var updatedCity = request.ToCity();
        var updated = await _cityService.UpdateAsync(updatedCity);

        if (!updated)
        {
            var message = $"Error updating city: {JsonSerializer.Serialize(updatedCity)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        updatedCity = await _cityService.GetByIdAsync(updatedCity.Id);
        var cityResponse = updatedCity!.ToCityResponse();
        return Ok(cityResponse);
    }

    [HttpDelete(ApiRoutesV1.Cities.Delete.Url)]
    public async Task<IActionResult> DeleteCity([FromRoute] Guid cityId)
    {
        if (!await _cityService.ExistsAsync(cityId)) return NotFound();

        var deleted = await _cityService.DeleteAsync(cityId);

        if (!deleted)
        {
            var message = $"Error deleting city with id {cityId}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(City), message));
        }

        return NoContent();
    }
}
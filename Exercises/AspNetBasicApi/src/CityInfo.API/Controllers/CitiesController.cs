using System.Text.Json;
using CityInfo.API.Attributes;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using CityInfo.API.Validators.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService ?? throw new NullReferenceException(nameof(cityService));
    }

    [HttpGet]
    public async Task<IActionResult> GetCities()
    {
        var cities = await _cityService.GetAllAsync();

        var citiesResponse = cities.Select(x => x.ToExtendedCityResponse());

        return Ok(citiesResponse);
    }

    [HttpGet("{cityId:guid}", Name = nameof(GetCity))]
    public async Task<IActionResult> GetCity([FromRoute] Guid cityId)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var cityResponse = city.ToExtendedCityResponse();

        return Ok(cityResponse);
    }

    [HttpPost]
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

        return CreatedAtRoute(nameof(GetCity),
            new { cityId = city.Id }, cityResponse);
    }

    [HttpPut("{cityId:guid}")]
    public async Task<IActionResult> UpdateCity([FromMultiSource] UpdateCityRequest request)
    {
        var city = await _cityService.GetByIdAsync(request.Id);
        if (city is null) return NotFound();

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

    [HttpDelete("{cityId:guid}")]
    public async Task<IActionResult> DeleteCity([FromRoute] Guid cityId)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var deleted = await _cityService.DeleteAsync(city);

        if (!deleted)
        {
            var message = $"Error deleting city: {JsonSerializer.Serialize(city)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(city), message));
        }

        return NoContent();
    }
}
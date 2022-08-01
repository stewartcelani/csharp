using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CityInfo.API.Attributes;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using CityInfo.API.Validators.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
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

    [HttpGet("{cityId:guid}")]
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

        return CreatedAtAction(nameof(GetCity),
            new { cityId = cityResponse.Id }, cityResponse);
    }

    [HttpPut("{cityId:guid}")]
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

    [HttpDelete("{cityId:guid}")]
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
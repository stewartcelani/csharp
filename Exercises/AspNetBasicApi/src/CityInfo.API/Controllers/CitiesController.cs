using CityInfo.API.Contracts.Responses;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
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
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
    {
        var cities = await _cityService.GetAllAsync();
        
        var citiesResponse = cities.Select(x => x.ToCityResponse());
        
        return Ok(citiesResponse);
    }
    
    [HttpGet("{cityId:guid}")]
    public async Task<ActionResult<CityResponse>> GetCity([FromRoute] Guid cityId)
    {
        var city = await _cityService.GetByIdAsync(cityId);
        
        if (city is null) return NotFound();
        
        var cityResponse = city.ToCityResponse();
        
        return Ok(cityResponse);
    }
}
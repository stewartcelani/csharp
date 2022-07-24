using CityInfo.API.Contracts.Responses;
using CityInfo.API.Data;
using CityInfo.API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public CitiesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityResponse>> GetCities()
    {
        var cities = _dbContext.Cities;
        var citiesResponse = cities.Select(x => x.ToCityResponse()); 
        return Ok(citiesResponse);
    }
    
    [HttpGet("{cityId:guid}")]
    public ActionResult<CityResponse> GetCity([FromRoute] Guid cityId)
    {
        var city = _dbContext.Cities.SingleOrDefault(x => x.Id == cityId);
        
        if (city is null) return NotFound();
        
        var cityResponse = city.ToCityResponse();
        
        return Ok(cityResponse);
    }
}
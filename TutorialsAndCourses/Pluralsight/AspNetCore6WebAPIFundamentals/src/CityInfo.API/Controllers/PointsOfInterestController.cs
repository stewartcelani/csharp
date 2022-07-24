using CityInfo.API.Attributes;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Data;
using CityInfo.API.Mappers;
using CityInfo.API.Validation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId:guid}/pointsofinterest")]
public class PointsOfInterestController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public PointsOfInterestController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestResponse>> GetPointsOfInterest([FromRoute] Guid cityId)
    {
        var city = _dbContext.Cities.SingleOrDefault(x => x.Id == cityId);

        if (city is null) return NotFound();

        var pointsOfInterestResponse = city.PointsOfInterest.Select(x => x.ToPointOfInterestResponse());

        return Ok(pointsOfInterestResponse);
    }

    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    [HttpGet("{pointOfInterestId:guid}", Name = nameof(GetPointOfInterest))]
    public ActionResult<PointOfInterestResponse> GetPointOfInterest([FromRoute] Guid cityId,
        [FromRoute] Guid pointOfInterestId)
    {
        var city = _dbContext.Cities.SingleOrDefault(x => x.Id == cityId);

        if (city is null) return NotFound();

        var pointOfInterest = city.PointsOfInterest.SingleOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest is null) return NotFound();

        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        return Ok(pointOfInterestResponse);
    }

    [HttpPost]
    public ActionResult<PointOfInterestResponse> CreatePointOfInterest([FromRoute] Guid cityId,
        [FromBody] CreatePointOfInterestRequest request)
    {
        var city = _dbContext.Cities.SingleOrDefault(x => x.Id == cityId);

        if (city is null) return NotFound();

        var pointOfInterest = request.ToPointOfInterest();

        city.PointsOfInterest.Add(pointOfInterest);

        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        return CreatedAtRoute(nameof(GetPointOfInterest),
            new { cityId = cityId, pointOfInterestId = pointOfInterestResponse.Id }, pointOfInterestResponse);
    }

    // ReSharper disable once RouteTemplates.RouteParameterIsNotPassedToMethod
    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    // ReSharper disable once RouteTemplates.MethodMissingRouteParameters
    [HttpPut("{pointOfInterestId:guid}")]
    public ActionResult<PointOfInterestResponse> UpdatePointOfInterest([FromRoute] Guid cityId,
        [FromMultiSource] UpdatePointOfInterestRequest request)
    {
        var city = _dbContext.Cities.FirstOrDefault(x => x.Id == cityId);

        if (city is null) return NotFound();

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == request.Id);

        if (pointOfInterest is null) return NotFound();

        var updatedPointOfInterest = request.ToPointOfInterest();

        city.PointsOfInterest.Remove(pointOfInterest);
        city.PointsOfInterest.Add(updatedPointOfInterest);

        var updatedPointOfInterestResponse = updatedPointOfInterest.ToPointOfInterestResponse();
        return Ok(updatedPointOfInterestResponse);
    }

    [HttpPatch("{pointOfInterestId:guid}")]
    public ActionResult PartiallyUpdatePointOfInterest([FromRoute] Guid cityId,
        [FromRoute] Guid pointOfInterestId,
        JsonPatchDocument<CreatePointOfInterestRequest> patchDocument)
    {
        var city = _dbContext.Cities.FirstOrDefault(x => x.Id == cityId);

        if (city is null) return NotFound();

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest is null) return NotFound();

        var createPointOfInterestRequest = pointOfInterest.ToCreatePointOfInterestRequest();
        
        patchDocument.ApplyTo(createPointOfInterestRequest);

        var validationResult = new CreatePointOfInterestRequestValidator().Validate(createPointOfInterestRequest);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var updatePointOfInterestRequest = new UpdatePointOfInterestRequest
        {
            Id = pointOfInterestId,
            CreatePointOfInterestRequest = createPointOfInterestRequest
        };
        
        var updatedPointOfInterest = updatePointOfInterestRequest.ToPointOfInterest();

        city.PointsOfInterest.Remove(pointOfInterest);
        city.PointsOfInterest.Add(updatedPointOfInterest);

        var updatedPointOfInterestResponse = updatedPointOfInterest.ToPointOfInterestResponse();
        return Ok(updatedPointOfInterestResponse);
    }
}
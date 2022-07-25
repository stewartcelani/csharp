using System.Text.Json;
using CityInfo.API.Attributes;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Logging;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using CityInfo.API.Validators;
using CityInfo.API.Validators.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId:guid}/pointsofinterest")]
public class PointsOfInterestController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ILoggerAdapter<PointsOfInterestController> _logger;

    public PointsOfInterestController(ILoggerAdapter<PointsOfInterestController> logger, ICityService cityService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestResponse>>> GetPointsOfInterest([FromRoute] Guid cityId)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var pointsOfInterestResponse = city.PointsOfInterest.Select(x => x.ToPointOfInterestResponse());

        return Ok(pointsOfInterestResponse);
    }

    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    [HttpGet("{pointOfInterestId:guid}", Name = nameof(GetPointOfInterest))]
    public async Task<ActionResult<PointOfInterestResponse>> GetPointOfInterest([FromRoute] Guid cityId,
        [FromRoute] Guid pointOfInterestId)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null)
        {
            _logger.LogInformation("City with id {cityId} wasn't found when accessing points of interest", cityId);
            return NotFound();
        }

        var pointOfInterest = city.PointsOfInterest.SingleOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest is null) return NotFound();

        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        return Ok(pointOfInterestResponse);
    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestResponse>> CreatePointOfInterest([FromRoute] Guid cityId,
        [FromBody] CreatePointOfInterestRequest request)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var pointOfInterest = request.ToPointOfInterest();

        var created = await _cityService.CreatePointOfInterestAsync(city, pointOfInterest);
        
        if (!created)
        {
            var message = $"Error creating point of interest: {JsonSerializer.Serialize(pointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(pointOfInterest), message));
        }

        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        return CreatedAtRoute(nameof(GetPointOfInterest),
            new { cityId = cityId, pointOfInterestId = pointOfInterestResponse.Id }, pointOfInterestResponse);
    }

    // ReSharper disable once RouteTemplates.RouteParameterIsNotPassedToMethod
    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    // ReSharper disable once RouteTemplates.MethodMissingRouteParameters
    [HttpPut("{pointOfInterestId:guid}")]
    public async Task<ActionResult<PointOfInterestResponse>> UpdatePointOfInterest([FromRoute] Guid cityId,
        [FromMultiSource] UpdatePointOfInterestRequest request)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == request.Id);

        if (pointOfInterest is null) return NotFound();

        var updatedPointOfInterest = request.ToPointOfInterest();

        var updated = await _cityService.UpdatePointOfInterestAsync(city, updatedPointOfInterest);

        if (!updated)
        {
            var message = $"Error updating point of interest: {JsonSerializer.Serialize(updatedPointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(updatedPointOfInterest), message));
        }

        var pointOfInterestResponse = updatedPointOfInterest.ToPointOfInterestResponse();
        return Ok(pointOfInterestResponse);
    }

    [HttpPatch("{pointOfInterestId:guid}")]
    public async Task<ActionResult<PointOfInterestResponse>> PartiallyUpdatePointOfInterest([FromRoute] Guid cityId,
        [FromRoute] Guid pointOfInterestId,
        JsonPatchDocument<CreatePointOfInterestRequest> patchDocument)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest is null) return NotFound();

        var updatePointOfInterestRequest = PatchPointOfInterest(patchDocument, pointOfInterest);

        var updatedPointOfInterest = updatePointOfInterestRequest.ToPointOfInterest();

        var updated = await _cityService.UpdatePointOfInterestAsync(city, updatedPointOfInterest);

        if (!updated)
        {
            var message = $"Error updating point of interest: {JsonSerializer.Serialize(updatedPointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(updatedPointOfInterest), message));
        }

        var pointOfInterestResponse = updatedPointOfInterest.ToPointOfInterestResponse();
        return Ok(pointOfInterestResponse);
    }

    [HttpDelete("{pointOfInterestId:guid}")]
    public async Task<IActionResult> DeletePointOfInterest([FromRoute] Guid cityId, [FromRoute] Guid pointOfInterestId)
    {
        var city = await _cityService.GetByIdAsync(cityId);

        if (city is null) return NotFound();

        var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

        if (pointOfInterest is null) return NotFound();

        var deleted = await _cityService.DeletePointOfInterestAsync(pointOfInterest.Id);

        if (!deleted)
        {
            var message = $"Error deleting point of interest: {JsonSerializer.Serialize(pointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(pointOfInterest), message));
        }

        return NoContent();
    }

    private static UpdatePointOfInterestRequest PatchPointOfInterest(
        JsonPatchDocument<CreatePointOfInterestRequest> patchDocument,
        PointOfInterest pointOfInterest)
    {
        var createPointOfInterestRequest = pointOfInterest.ToCreatePointOfInterestRequest();

        try
        {
            patchDocument.ApplyTo(createPointOfInterestRequest);
        }
        catch (JsonPatchException ex)
        {
            throw new ValidationException(ex.Message,
                ValidationFailureHelper.Generate(ex.FailedOperation.path.Replace(@"/", ""),
                    ex.Message));
        }

        var validationResult = new CreatePointOfInterestRequestValidator().Validate(createPointOfInterestRequest);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return new UpdatePointOfInterestRequest
        {
            Id = pointOfInterest.Id,
            CreatePointOfInterestRequest = createPointOfInterestRequest
        };
    }
}
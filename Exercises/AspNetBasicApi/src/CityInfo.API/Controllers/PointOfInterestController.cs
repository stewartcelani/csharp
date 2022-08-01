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
using CityInfo.API.Validators;
using CityInfo.API.Validators.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities/{cityId:guid}/pointsofinterest")]
public class PointOfInterestController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IPointOfInterestService _pointOfInterestService;

    public PointOfInterestController(IPointOfInterestService pointOfInterestService, ICityService cityService)
    {
        _pointOfInterestService =
            pointOfInterestService ?? throw new ArgumentNullException(nameof(pointOfInterestService));
        _cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
    }

    [HttpGet]
    public async Task<IActionResult> GetPointsOfInterest([FromRoute] Guid cityId)
    {
        if (!await _cityService.ExistsAsync(cityId)) return NotFound();

        var pointsOfInterest = await _pointOfInterestService.GetAllAsync(cityId);

        var pointsOfInterestResponse = pointsOfInterest.Select(x => x.ToPointOfInterestResponse());

        return Ok(pointsOfInterestResponse);
    }

    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    [HttpGet("{pointOfInterestId:guid}")]
    public async Task<IActionResult> GetPointOfInterest([FromRoute] Guid pointOfInterestId)
    {
        var pointOfInterest = await _pointOfInterestService.GetByIdAsync(pointOfInterestId);

        if (pointOfInterest is null) return NotFound();

        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        return Ok(pointOfInterestResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePointOfInterest([FromRoute] Guid cityId,
        [FromBody] CreatePointOfInterestRequest request)
    {
        if (!await _cityService.ExistsAsync(cityId)) return NotFound();

        var pointOfInterest = request.ToPointOfInterest();

        var created = await _pointOfInterestService.CreateAsync(cityId, pointOfInterest);

        if (!created)
        {
            var message = $"Error creating point of interest: {JsonSerializer.Serialize(pointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(PointOfInterest), message));
        }

        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        return CreatedAtAction(nameof(GetPointOfInterest),
            new { cityId, pointOfInterestId = pointOfInterestResponse.Id }, pointOfInterestResponse);
    }

    // ReSharper disable once RouteTemplates.RouteParameterIsNotPassedToMethod
    // ReSharper disable once RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
    // ReSharper disable once RouteTemplates.MethodMissingRouteParameters
    [HttpPut("{pointOfInterestId:guid}")]
    public async Task<IActionResult> UpdatePointOfInterest([FromRoute] Guid cityId,
        [FromMultiSource] UpdatePointOfInterestRequest request)
    {
        if (!await _cityService.ExistsAsync(cityId)) return NotFound();
        if (!await _pointOfInterestService.ExistsAsync(request.Id)) return NotFound();

        var updatedPointOfInterest = request.ToPointOfInterest();

        var updated = await _pointOfInterestService.UpdateAsync(cityId, updatedPointOfInterest);

        if (!updated)
        {
            var message = $"Error updating point of interest: {JsonSerializer.Serialize(updatedPointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(PointOfInterest), message));
        }

        var pointOfInterestResponse = updatedPointOfInterest.ToPointOfInterestResponse();
        return Ok(pointOfInterestResponse);
    }

    [HttpPatch("{pointOfInterestId:guid}")]
    public async Task<IActionResult> PartiallyUpdatePointOfInterest([FromRoute] Guid cityId,
        [FromRoute] Guid pointOfInterestId,
        JsonPatchDocument<CreatePointOfInterestRequest> patchDocument)
    {
        if (!await _cityService.ExistsAsync(cityId)) return NotFound();

        var pointOfInterest = await _pointOfInterestService.GetByIdAsync(pointOfInterestId);
        if (pointOfInterest is null) return NotFound();

        var updatePointOfInterestRequest = PatchPointOfInterest(patchDocument, pointOfInterest);

        var updatedPointOfInterest = updatePointOfInterestRequest.ToPointOfInterest();

        var updated = await _pointOfInterestService.UpdateAsync(cityId, updatedPointOfInterest);

        if (!updated)
        {
            var message = $"Error updating point of interest: {JsonSerializer.Serialize(updatedPointOfInterest)}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(PointOfInterest), message));
        }

        var pointOfInterestResponse = updatedPointOfInterest.ToPointOfInterestResponse();
        return Ok(pointOfInterestResponse);
    }

    [HttpDelete("{pointOfInterestId:guid}")]
    public async Task<IActionResult> DeletePointOfInterest([FromRoute] Guid pointOfInterestId)
    {
        if (!await _pointOfInterestService.ExistsAsync(pointOfInterestId)) return NotFound();

        var deleted = await _pointOfInterestService.DeleteAsync(pointOfInterestId);

        if (!deleted)
        {
            var message = $"Error deleting point of interest with id {pointOfInterestId}";
            throw new ApiException(message, ValidationFailureHelper.Generate(nameof(PointOfInterest), message));
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
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        return new UpdatePointOfInterestRequest
        {
            Id = pointOfInterest.Id,
            CreatePointOfInterestRequest = createPointOfInterestRequest
        };
    }
}
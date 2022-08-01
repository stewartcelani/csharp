using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Controllers;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace CityInfo.API.Tests.Unit.Controllers.PointsOfInterestControllerTests;

[ExcludeFromCodeCoverage]
public class UpdatePointsOfInterestControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly IPointOfInterestService _pointOfInterestService = Substitute.For<IPointOfInterestService>();
    private readonly PointOfInterestController _sut;

    public UpdatePointsOfInterestControllerTests()
    {
        _sut = new PointOfInterestController(_pointOfInterestService, _cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task
        UpdatePointOfInterest_ShouldUpdatePointOfInterest_WhenUpdatePointOfInterestRequestIsValid()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var updatePointOfInterestRequest = new UpdatePointOfInterestRequest
        {
            Id = Guid.NewGuid(),
            CreatePointOfInterestRequest = new CreatePointOfInterestRequest
            {
                Name = "Point Of Interest Name",
                Description = "Point Of Interest description."
            }
        };
        var pointOfInterest = updatePointOfInterestRequest.ToPointOfInterest();
        _cityService.ExistsAsync(existingCityId).Returns(true);
        _pointOfInterestService.ExistsAsync(pointOfInterest.Id).Returns(true);
        _pointOfInterestService.UpdateAsync(existingCityId, Arg.Do<PointOfInterest>(x => pointOfInterest = x))
            .Returns(true);

        // Act
        var result =
            (OkObjectResult)await _sut.UpdatePointOfInterest(existingCityId, updatePointOfInterestRequest);

        // Assert
        var expectedPointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();
        result.StatusCode.Should().Be(200);
        result.Value.As<PointOfInterestResponse>().Should().BeEquivalentTo(expectedPointOfInterestResponse);
    }

    [Fact]
    public async Task
        UpdatePointOfInterest_ShouldThrowApiException_WhenUpdatePointOfInterestRequestIsInvalid()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var updatePointOfInterestRequest = new UpdatePointOfInterestRequest
        {
            Id = Guid.NewGuid(),
            CreatePointOfInterestRequest = new CreatePointOfInterestRequest
            {
                Name = "Point Of Interest Name",
                Description = "Point Of Interest description."
            }
        };
        var pointOfInterest = updatePointOfInterestRequest.ToPointOfInterest();
        _cityService.ExistsAsync(existingCityId).Returns(true);
        _pointOfInterestService.ExistsAsync(pointOfInterest.Id).Returns(true);
        _pointOfInterestService.UpdateAsync(existingCityId, Arg.Any<PointOfInterest>()).Returns(false);

        // Act
        var action = async () => await _sut.UpdatePointOfInterest(existingCityId, updatePointOfInterestRequest);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage("Error updating point of interest*");
    }

    [Fact]
    public async Task
        UpdatePointOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var updatePointOfInterestRequest = new UpdatePointOfInterestRequest
        {
            Id = Guid.NewGuid(),
            CreatePointOfInterestRequest = new CreatePointOfInterestRequest
            {
                Name = "Point Of Interest Name",
                Description = "Point Of Interest description."
            }
        };
        _cityService.ExistsAsync(existingCityId).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.UpdatePointOfInterest(existingCityId, updatePointOfInterestRequest);

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task
        UpdatePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var updatePointOfInterestRequest = new UpdatePointOfInterestRequest
        {
            Id = Guid.NewGuid(),
            CreatePointOfInterestRequest = new CreatePointOfInterestRequest
            {
                Name = "Point Of Interest Name",
                Description = "Point Of Interest description."
            }
        };
        var pointOfInterest = updatePointOfInterestRequest.ToPointOfInterest();
        _cityService.ExistsAsync(existingCityId).Returns(true);
        _pointOfInterestService.ExistsAsync(pointOfInterest.Id).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.UpdatePointOfInterest(existingCityId, updatePointOfInterestRequest);

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task
        PartiallyUpdatePointOfInterest_ShouldPatchPointOfInterest_WhenPathDocumentIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        var existingName = pointOfInterest.Name;
        const string updatedName = "New Name";
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", existingName, updatedName));
        _cityService.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestService.GetByIdAsync(pointOfInterest.Id).Returns(pointOfInterest);
        _pointOfInterestService.UpdateAsync(city.Id, Arg.Do<PointOfInterest>(x => pointOfInterest = x)).Returns(true);

        // Act
        var result =
            (OkObjectResult)await _sut.PartiallyUpdatePointOfInterest(city.Id, pointOfInterest.Id, patchDocument);

        // Assert
        var expectedPointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();
        result.StatusCode.Should().Be(200);
        var pointOfInterestResponse = result.Value.As<PointOfInterestResponse>();
        pointOfInterestResponse.Should().BeEquivalentTo(expectedPointOfInterestResponse);
        pointOfInterestResponse.Name.Should().Be(updatedName);
    }

    [Fact]
    public async Task
        PartiallyUpdatePointOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        var existingName = pointOfInterest.Name;
        const string updatedName = "New Name";
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", existingName, updatedName));
        _cityService.ExistsAsync(city.Id).Returns(false);

        // Act
        var result =
            (NotFoundResult)await _sut.PartiallyUpdatePointOfInterest(city.Id, pointOfInterest.Id, patchDocument);

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task
        PartiallyUpdatePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        var existingName = pointOfInterest.Name;
        const string updatedName = "New Name";
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", existingName, updatedName));
        _cityService.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestService.GetByIdAsync(pointOfInterest.Id).ReturnsNull();

        // Act
        var result =
            (NotFoundResult)await _sut.PartiallyUpdatePointOfInterest(city.Id, pointOfInterest.Id, patchDocument);

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task
        PartiallyUpdatePointOfInterest_ShouldThrowValidationException_WhenPatchDocumentPathIsInvalid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        var existingName = pointOfInterest.Name;
        const string updatedName = "New Name";
        const string invalidPropertyName = "invalidPropertyName";
        patchDocument.Operations.Add(new Operation<CreatePointOfInterestRequest>("replace", $"/{invalidPropertyName}",
            existingName, updatedName));
        _cityService.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestService.GetByIdAsync(pointOfInterest.Id).Returns(pointOfInterest);
        _pointOfInterestService.UpdateAsync(city.Id, Arg.Do<PointOfInterest>(x => pointOfInterest = x)).Returns(true);

        // Act
        var action = async () => await _sut.PartiallyUpdatePointOfInterest(city.Id, pointOfInterest.Id, patchDocument);

        // Assert
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage($"The target location specified by path segment '{invalidPropertyName}' was not found.");
    }

    [Fact]
    public async Task
        PartiallyUpdatePointOfInterest_ShouldThrowApiException_WhenThereIsAnErrorUpdatingPointOfInterest()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        var existingName = pointOfInterest.Name;
        const string updatedName = "New Name";
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", existingName, updatedName));
        _cityService.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestService.GetByIdAsync(pointOfInterest.Id).Returns(pointOfInterest);
        _pointOfInterestService.UpdateAsync(city.Id, Arg.Do<PointOfInterest>(x => pointOfInterest = x)).Returns(false);

        // Act
        var action = async () => await _sut.PartiallyUpdatePointOfInterest(city.Id, pointOfInterest.Id, patchDocument);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage("Error updating point of interest*");
    }
}
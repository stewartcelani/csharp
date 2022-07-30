using System;
using System.Collections.Generic;
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

namespace CityInfo.API.Tests.Unit.Controllers;

[ExcludeFromCodeCoverage]
public class PointsOfInterestControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly Faker<PointOfInterest> _pointOfInterestGenerator;
    private readonly IPointOfInterestService _pointOfInterestService = Substitute.For<IPointOfInterestService>();
    private readonly PointsOfInterestController _sut;

    public PointsOfInterestControllerTests()
    {
        _sut = new PointsOfInterestController(_pointOfInterestService, _cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
        _pointOfInterestGenerator = SharedTestContext.PointOfInterestGenerator;
    }

    [Fact]
    public async Task GetPointsOfInterest_ShouldReturnEmptyList_WhenCityHasNoPointsOfInterest()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityService.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestService.GetAllAsync(city.Id).Returns(Enumerable.Empty<PointOfInterest>());

        // Act
        var result = (OkObjectResult)await _sut.GetPointsOfInterest(city.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<PointOfInterestResponse>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetPointsOfInterest_ShouldPointsOfInterest_WhenCityHasPointsOfInterest()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityService.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestService.GetAllAsync(city.Id).Returns(city.PointsOfInterest);
        var expectedPointOfInterestResponse = city.PointsOfInterest.Select(x => x.ToPointOfInterestResponse());

        // Act
        var result = (OkObjectResult)await _sut.GetPointsOfInterest(city.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<PointOfInterestResponse>>().Should()
            .BeEquivalentTo(expectedPointOfInterestResponse);
        result.Value.As<IEnumerable<PointOfInterestResponse>>().Should().HaveCount(city.PointsOfInterest.Count);
    }

    [Fact]
    public async Task GetPointOfInterest_ShouldReturnPointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        _pointOfInterestService.GetByIdAsync(pointOfInterest.Id).Returns(pointOfInterest);
        var pointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetPointOfInterest(pointOfInterest.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(pointOfInterestResponse);
    }

    [Fact]
    public async Task GetPointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        _pointOfInterestService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult)await _sut.GetPointOfInterest(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task
        CreatePointOfInterest_ShouldCreatePointOfInterest_WhenCreatePointOfInterestRequestIsValid()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var createPointOfInterestRequest = new CreatePointOfInterestRequest
        {
            Name = "Point Of Interest Name",
            Description = "Point Of Interest description."
        };
        var pointOfInterest = createPointOfInterestRequest.ToPointOfInterest();
        _cityService.ExistsAsync(existingCityId).Returns(true);
        _pointOfInterestService.CreateAsync(existingCityId, Arg.Do<PointOfInterest>(x => pointOfInterest = x))
            .Returns(true);

        // Act
        var result =
            (CreatedAtActionResult)await _sut.CreatePointOfInterest(existingCityId, createPointOfInterestRequest);

        // Assert
        var expectedPointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();
        result.StatusCode.Should().Be(201);
        result.Value.As<PointOfInterestResponse>().Should().BeEquivalentTo(expectedPointOfInterestResponse);
        result.RouteValues!["pointOfInterestId"].Should().BeEquivalentTo(pointOfInterest.Id);
    }

    [Fact]
    public async Task CreatePointOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var createPointOfInterestRequest = new CreatePointOfInterestRequest
        {
            Name = "Point Of Interest Name",
            Description = "Point Of Interest description."
        };
        _cityService.ExistsAsync(existingCityId).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.CreatePointOfInterest(existingCityId, createPointOfInterestRequest);

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task CreatePointOfInterest_ShouldThrowApiException_WhenCreatePointOfInterestRequestIsInvalid()
    {
        // Arrange
        var existingCityId = Guid.NewGuid();
        var createPointOfInterestRequest = new CreatePointOfInterestRequest
        {
            Name = "Point Of Interest Name",
            Description = "Point Of Interest description."
        };
        _cityService.ExistsAsync(existingCityId).Returns(true);
        _pointOfInterestService.CreateAsync(existingCityId, createPointOfInterestRequest.ToPointOfInterest())
            .Returns(false);

        // Act
        var action = async () => await _sut.CreatePointOfInterest(existingCityId, createPointOfInterestRequest);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage("Error creating point of interest*");
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

    [Fact]
    public async Task DeletePointOfInterest_ShouldDeletePointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _pointOfInterestService.ExistsAsync(id).Returns(true);
        _pointOfInterestService.DeleteAsync(id).Returns(true);

        // Act
        var result = (NoContentResult)await _sut.DeletePointOfInterest(id);

        // Assert
        result.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeletePointOfInterest_ShouldThrowApiException_WhenThereIsAnErrorDeletingPointOfInterest()
    {
        // Arrange
        var id = Guid.NewGuid();
        _pointOfInterestService.ExistsAsync(id).Returns(true);
        _pointOfInterestService.DeleteAsync(id).Returns(false);

        // Act
        var action = async () => await _sut.DeletePointOfInterest(id);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage($"Error deleting point of interest with id {id}");
    }

    [Fact]
    public async Task DeletePointOfInterest_ReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _pointOfInterestService.ExistsAsync(id).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.DeletePointOfInterest(id);

        // Assert
        result.StatusCode.Should().Be(404);
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
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
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers.PointsOfInterestControllerTests;

[ExcludeFromCodeCoverage]
public class CreatePointsOfInterestControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly Faker<PointOfInterest> _pointOfInterestGenerator;
    private readonly IPointOfInterestService _pointOfInterestService = Substitute.For<IPointOfInterestService>();
    private readonly PointsOfInterestController _sut;

    public CreatePointsOfInterestControllerTests()
    {
        _sut = new PointsOfInterestController(_pointOfInterestService, _cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
        _pointOfInterestGenerator = SharedTestContext.PointOfInterestGenerator;
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
}
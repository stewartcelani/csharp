using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Controllers;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers.PointsOfInterestControllerTests;

[ExcludeFromCodeCoverage]
public class GetPointsOfInterestControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly IPointOfInterestService _pointOfInterestService = Substitute.For<IPointOfInterestService>();
    private readonly PointOfInterestController _sut;

    public GetPointsOfInterestControllerTests()
    {
        _sut = new PointOfInterestController(_pointOfInterestService, _cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
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
}
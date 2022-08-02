using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers.CityController;

[ExcludeFromCodeCoverage]
public class GetCityControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly API.Controllers.CityController _sut;


    public GetCityControllerTests()
    {
        _sut = new API.Controllers.CityController(_cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task GetCities_ShouldReturnEmptyList_WhenNoCitiesExist()
    {
        // Arrange
        _cityService.GetAsync().Returns(Enumerable.Empty<City>());

        // Act
        var result = (OkObjectResult)await _sut.GetCities();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<ExtendedCityResponse>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetCities_ShouldReturnCities_WhenCitiesExist()
    {
        // Arrange
        var cities = _cityGenerator.Generate(5);
        _cityService.GetAsync().Returns(cities);
        var citiesResponse = cities.Select(x => x.ToExtendedCityResponse());

        // Act
        var result = (OkObjectResult)await _sut.GetCities();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<ExtendedCityResponse>>().Should().BeEquivalentTo(citiesResponse);
        result.Value.As<IEnumerable<ExtendedCityResponse>>().Should().HaveCount(cities.Count);
    }

    [Fact]
    public async Task GetCity_ShouldReturnCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityService.GetByIdAsync(city.Id).Returns(city);
        var cityResponse = city.ToExtendedCityResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetCity(city.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(cityResponse);
    }

    [Fact]
    public async Task GetCity_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        _cityService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult)await _sut.GetCity(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(404);
    }
}
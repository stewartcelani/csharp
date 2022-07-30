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
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers;

[ExcludeFromCodeCoverage]
public class CitiesControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly CitiesController _sut;


    public CitiesControllerTests()
    {
        _sut = new CitiesController(_cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task GetCities_ShouldReturnEmptyList_WhenNoCitiesExist()
    {
        // Arrange
        _cityService.GetAllAsync().Returns(Enumerable.Empty<City>());

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
        _cityService.GetAllAsync().Returns(cities);
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

    [Fact]
    public async Task CreateCity_ShouldCreateCity_WhenCreateCityRequestIsValid()
    {
        // Arrange
        var createCityRequest = new CreateCityRequest
        {
            Name = "City Name",
            Description = "City description."
        };
        var city = createCityRequest.ToCity();
        _cityService.CreateAsync(Arg.Do<City>(x => city = x)).Returns(true);

        // Act
        var result = (CreatedAtActionResult)await _sut.CreateCity(createCityRequest);

        // Assert
        var expectedCityResponse = city.ToCityResponse();
        result.StatusCode.Should().Be(201);
        result.Value.As<CityResponse>().Should().BeEquivalentTo(expectedCityResponse);
        result.RouteValues!["cityId"].Should().BeEquivalentTo(city.Id);
    }

    /// <summary>
    ///     Note the ApiException is translated into a BadRequest 400 with error message by middleware
    /// </summary>
    [Fact]
    public async Task CreateCity_ShouldThrowApiException_WhenCreateCityRequestIsInvalid()
    {
        // Arrange
        _cityService.CreateAsync(Arg.Any<City>()).Returns(false);

        // Act
        var action = async () => await _sut.CreateCity(new CreateCityRequest());

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage("Error creating city*");
    }

    [Fact]
    public async Task UpdateCity_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        _cityService.ExistsAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.UpdateCity(new UpdateCityRequest());

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateCity_ShouldUpdateCity_WhenUpdateCityRequestIsValid()
    {
        // Arrange
        var updateCityRequest = new UpdateCityRequest
        {
            Id = Guid.NewGuid(),
            CreateCityRequest = new CreateCityRequest
            {
                Name = "City Name",
                Description = "City description."
            }
        };
        var city = updateCityRequest.ToCity();
        _cityService.ExistsAsync(city.Id).Returns(true);
        _cityService.UpdateAsync(Arg.Do<City>(x => city = x)).Returns(true);
        _cityService.GetByIdAsync(city.Id).Returns(city);

        // Act
        var result = (OkObjectResult)await _sut.UpdateCity(updateCityRequest);

        // Assert
        var expectedCityResponse = city.ToCityResponse();
        result.StatusCode.Should().Be(200);
        result.Value.As<CityResponse>().Should().BeEquivalentTo(expectedCityResponse);
    }

    [Fact]
    public async Task UpdateCity_ShouldThrowApiException_WhenUpdateCityRequestIsInvalid()
    {
        // Arrange
        var updateCityRequest = new UpdateCityRequest
        {
            Id = Guid.NewGuid(),
            CreateCityRequest = new CreateCityRequest
            {
                Name = "City Name",
                Description = "City description."
            }
        };
        _cityService.ExistsAsync(Arg.Any<Guid>()).Returns(true);
        _cityService.UpdateAsync(Arg.Any<City>()).Returns(false);

        // Act
        var action = async () => await _sut.UpdateCity(updateCityRequest);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage("Error updating city*");
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        _cityService.ExistsAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.DeleteCity(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task DeleteCity_ShouldDeleteCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityService.ExistsAsync(city.Id).Returns(true);
        _cityService.DeleteAsync(city.Id).Returns(true);

        // Act
        var result = (NoContentResult)await _sut.DeleteCity(city.Id);

        // Assert
        result.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteCity_ShouldThrowApiException_WhenThereIsAnErrorDeletingCity()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityService.ExistsAsync(city.Id).Returns(true);
        _cityService.DeleteAsync(city.Id).Returns(false);

        // Act
        var action = async () => await _sut.DeleteCity(city.Id);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage($"Error deleting city with id {city.Id}");
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Controllers;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers.CitiesControllerTests;

[ExcludeFromCodeCoverage]
public class DeleteCitiesControllerTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly CitiesController _sut;


    public DeleteCitiesControllerTests()
    {
        _sut = new CitiesController(_cityService);
        _cityGenerator = SharedTestContext.CityGenerator;
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
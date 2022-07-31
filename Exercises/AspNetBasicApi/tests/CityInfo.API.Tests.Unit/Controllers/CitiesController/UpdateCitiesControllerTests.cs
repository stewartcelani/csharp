using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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

namespace CityInfo.API.Tests.Unit.Controllers.CitiesControllerTests;

[ExcludeFromCodeCoverage]
public class UpdateCitiesControllerTests
{
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly CitiesController _sut;


    public UpdateCitiesControllerTests()
    {
        _sut = new CitiesController(_cityService);
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
}
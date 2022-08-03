using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.v1.Requests.Queries;
using CityInfo.API.Contracts.v1.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Filters;
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
    private readonly API.Controllers.v1.CityController _sut;


    public GetCityControllerTests()
    {
        _sut = new API.Controllers.v1.CityController(_cityService, new UriService("http://localhost/"));
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task GetCities_ShouldReturnEmptyList_WhenNoCitiesExist()
    {
        // Arrange
        var defaultPaginationQuery = new PaginationQuery();
        _cityService.GetAsync(
                Arg.Is<GetCitiesFilter>(x => x.Name == null),
                Arg.Is<PaginationFilter>(x =>
                    x.PageSize == defaultPaginationQuery.PageSize && x.PageNumber == defaultPaginationQuery.PageNumber))
            .Returns(Enumerable.Empty<City>());

        // Act
        var result = (OkObjectResult)await _sut.GetCities(new GetCitiesQuery(), new PaginationQuery());

        // Assert
        result.StatusCode.Should().Be(200);
        var pagedResponse = result.Value.As<PagedResponse<ExtendedCityResponse>>();
        pagedResponse.Data.Should().BeEmpty();
        pagedResponse.PageSize.Should().Be(defaultPaginationQuery.PageSize);
        pagedResponse.PageNumber.Should().Be(defaultPaginationQuery.PageNumber);
        pagedResponse.NextPage.Should().BeNull();
        pagedResponse.PreviousPage.Should().BeNull();
    }

    [Fact]
    public async Task GetCities_ShouldReturnCities_WhenCitiesExist()
    {
        // Arrange
        var cities = _cityGenerator.Generate(5);
        var defaultPaginationQuery = new PaginationQuery();
        _cityService.GetAsync(
                Arg.Is<GetCitiesFilter>(x => x.Name == null),
                Arg.Is<PaginationFilter>(x =>
                    x.PageSize == defaultPaginationQuery.PageSize && x.PageNumber == defaultPaginationQuery.PageNumber))
            .Returns(cities);
        var citiesResponse = cities.Select(x => x.ToExtendedCityResponse());

        // Act
        var result = (OkObjectResult)await _sut.GetCities(new GetCitiesQuery(), new PaginationQuery());

        // Assert
        result.StatusCode.Should().Be(200);
        var pagedResponse = result.Value.As<PagedResponse<ExtendedCityResponse>>();
        pagedResponse.Data.Should().BeEquivalentTo(citiesResponse);
        pagedResponse.Data.Should().HaveCount(cities.Count);
        pagedResponse.PageSize.Should().Be(defaultPaginationQuery.PageSize);
        pagedResponse.PageNumber.Should().Be(defaultPaginationQuery.PageNumber);
        pagedResponse.NextPage.Should().BeNull();
        pagedResponse.PreviousPage.Should().BeNull();
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
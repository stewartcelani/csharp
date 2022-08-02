using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CityInfo.API.Tests.Unit.Services.CityServiceTests;

[ExcludeFromCodeCoverage]
public class GetCityServiceTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly CityService _sut;

    public GetCityServiceTests()
    {
        _sut = new CityService(_cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenCityExists()
    {
        // Arrange
        _cityRepository.ExistsAsync(Arg.Any<Guid>()).Returns(true);

        // Act
        var result = await _sut.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenCityDoesNotExist()
    {
        // Arrange
        _cityRepository.ExistsAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = await _sut.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.GetAsync(city.Id, Arg.Any<IEnumerable<string>>()).Returns(cityEntity);

        // Act
        var result = await _sut.GetByIdAsync(city.Id);

        // Assert
        result.Should().BeEquivalentTo(city);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCityDoesNotExist()
    {
        // Arrange
        _cityRepository.GetAsync(Arg.Any<Guid>(), Arg.Any<IEnumerable<string?>>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCities_WhenAtLeastOneCityExists()
    {
        // Arrange
        var cities = _cityGenerator.Generate(2);
        _cityRepository.GetAsync(Arg.Any<Expression<Func<CityEntity, bool>>?>(), Arg.Any<IEnumerable<string>?>())
            .Returns(cities.Select(x => x.ToCityEntity()));

        // Act
        var result = await _sut.GetAsync();

        // Assert
        result.Should().BeEquivalentTo(cities);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCitiesExist()
    {
        // Arrange
        _cityRepository.GetAsync(Arg.Any<Expression<Func<CityEntity, bool>>?>(), Arg.Any<IEnumerable<string>?>())
            .Returns(Enumerable.Empty<CityEntity>());

        // Act
        var result = await _sut.GetAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.Services.CityServiceTests;

[ExcludeFromCodeCoverage]
public class DeleteCityServiceTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly CityService _sut;

    public DeleteCityServiceTests()
    {
        _sut = new CityService(_cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _cityRepository.DeleteAsync(city.Id).Returns(true);

        // Act
        var result = await _sut.DeleteAsync(city.Id);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_FromGuardClause_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityRepository.ExistsAsync(city.Id).Returns(false);
        _cityRepository.DeleteAsync(city.Id).Returns(false);

        // Act
        var result = await _sut.DeleteAsync(city.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenRepositoryCouldNotChangeDatabase()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _cityRepository.DeleteAsync(city.Id).Returns(false);

        // Act
        var result = await _sut.DeleteAsync(city.Id);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeFalse();
    }
}
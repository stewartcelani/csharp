using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.CityService;

[ExcludeFromCodeCoverage]
public class DeleteCityServiceTests
{
    private readonly Services.CityService _sut;
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly Faker<City> _cityGenerator;

    public DeleteCityServiceTests()
    {
        _sut = new Services.CityService(_cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _cityRepository.DeleteAsync(Arg.Do<CityEntity>(x => cityEntity = x)).Returns(true);

        // Act
        var result = await _sut.DeleteAsync(city);

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
        _cityRepository.DeleteAsync(Arg.Any<CityEntity>()).Returns(false);
        
        // Act
        var result = await _sut.DeleteAsync(city);

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
        _cityRepository.DeleteAsync(Arg.Do<CityEntity>(x => cityEntity = x)).Returns(false);
        
        // Act
        var result = await _sut.DeleteAsync(city);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeFalse();
    }
}
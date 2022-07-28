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
using ValidationException = FluentValidation.ValidationException;

namespace CityInfo.API.Tests.Unit.CityService;

[ExcludeFromCodeCoverage]
public class UpdateCityServiceTests
{
    private readonly Services.CityService _sut;
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly Faker<City> _cityGenerator;

    public UpdateCityServiceTests()
    {
        _sut = new Services.CityService(_cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCity_WhenCityIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _cityRepository.UpdateAsync(Arg.Do<CityEntity>(x => cityEntity = x)).Returns(true);

        // Act
        var result = await _sut.UpdateAsync(city);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityRepository.ExistsAsync(city.Id).Returns(false);

        // Act
        var action = async () => await _sut.UpdateAsync(city);

        // Assert
        await action.Should().ThrowAsync<ValidationException>().WithMessage($"Can not update city with id {city.Id} as it does not exist");
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenRepositoryCouldNotChangeDatabase()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _cityRepository.UpdateAsync(Arg.Do<CityEntity>(x => cityEntity = x)).Returns(false);

        // Act
        var result = await _sut.UpdateAsync(city);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeFalse();
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

namespace CityInfo.API.Tests.Unit.PointOfInterestService;

[ExcludeFromCodeCoverage]
public class UpdatePointOfInterestTests
{
    private readonly Services.PointOfInterestService _sut;
    private readonly IPointOfInterestRepository _pointOfInterestRepository = Substitute.For<IPointOfInterestRepository>();
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly Faker<City> _cityGenerator;
    private readonly Faker<PointOfInterest> _pointOfInterestGenerator;
    
    public UpdatePointOfInterestTests()
    {
        _sut = new Services.PointOfInterestService(_pointOfInterestRepository, _cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
        _pointOfInterestGenerator = SharedTestContext.PointOfInterestGenerator;
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdatePointOfInterest_PointOfInterestIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city.Id);
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id).Returns(true);
        _pointOfInterestRepository.UpdateAsync(Arg.Do<PointOfInterestEntity>(x => pointOfInterestEntity = x)).Returns(true);

        // Act
        var result = await _sut.UpdateAsync(city.Id, pointOfInterest);

        // Assert
        pointOfInterestEntity.Should().BeEquivalentTo(pointOfInterest);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        _cityRepository.ExistsAsync(city.Id).Returns(false);

        // Act
        var action = async () => await _sut.UpdateAsync(city.Id, pointOfInterest);

        // Assert
        await action.Should().ThrowAsync<ValidationException>().WithMessage($"City with id {city.Id} does not exist");
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id).Returns(false);

        // Act
        var action = async () => await _sut.UpdateAsync(city.Id, pointOfInterest);

        // Assert
        await action.Should().ThrowAsync<ValidationException>().WithMessage($"Can not update point of interest with id {pointOfInterest.Id} as it does not exist");
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenRepositoryCouldNotChangeDatabase()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city.Id);
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id).Returns(true);
        _pointOfInterestRepository.UpdateAsync(Arg.Do<PointOfInterestEntity>(x => pointOfInterestEntity = x)).Returns(false);

        // Act
        var result = await _sut.UpdateAsync(city.Id, pointOfInterest);

        // Assert
        pointOfInterestEntity.Should().BeEquivalentTo(pointOfInterest);
        result.Should().BeFalse();
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.Services.PointOfInterestServiceTests;

[ExcludeFromCodeCoverage]
public class DeletePointOfInterestTests
{
    private readonly Faker<City> _cityGenerator;
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();

    private readonly IPointOfInterestRepository _pointOfInterestRepository =
        Substitute.For<IPointOfInterestRepository>();

    private readonly PointOfInterestService _sut;

    public DeletePointOfInterestTests()
    {
        _sut = new PointOfInterestService(_pointOfInterestRepository, _cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city.Id);
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id).Returns(true);
        _pointOfInterestRepository.DeleteAsync(pointOfInterest.Id).Returns(true);

        // Act
        var result = await _sut.DeleteAsync(pointOfInterest.Id);

        // Assert
        pointOfInterestEntity.Should().BeEquivalentTo(pointOfInterest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_FromGuardClause_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestRepository.ExistsAsync(city.Id).Returns(false);

        // Act
        var result = await _sut.DeleteAsync(pointOfInterest.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenRepositoryCouldNotChangeDatabase()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = city.PointsOfInterest.First();
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city.Id);
        _cityRepository.ExistsAsync(city.Id).Returns(true);
        _pointOfInterestRepository.ExistsAsync(pointOfInterest.Id).Returns(true);
        _pointOfInterestRepository.DeleteAsync(pointOfInterest.Id).Returns(false);

        // Act
        var result = await _sut.DeleteAsync(pointOfInterest.Id);

        // Assert
        pointOfInterestEntity.Should().BeEquivalentTo(pointOfInterest);
        result.Should().BeFalse();
    }
}
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
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace CityInfo.API.Tests.Unit.PointOfInterestService;

[ExcludeFromCodeCoverage]
public class GetPointOfInterestTests
{
    private readonly Services.PointOfInterestService _sut;
    private readonly IPointOfInterestRepository _pointOfInterestRepository = Substitute.For<IPointOfInterestRepository>();
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly Faker<City> _cityGenerator;
    private readonly Faker<PointOfInterest> _pointOfInterestGenerator;

    public GetPointOfInterestTests()
    {
        _sut = new Services.PointOfInterestService(_pointOfInterestRepository, _cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
        _pointOfInterestGenerator = SharedTestContext.PointOfInterestGenerator;
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenPointOfInterestExists()
    {
        // Arrange
        _pointOfInterestRepository.ExistsAsync(Arg.Any<Guid>()).Returns(true);

        // Act
        var result = await _sut.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        _pointOfInterestRepository.ExistsAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = await _sut.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterest = _pointOfInterestGenerator.Generate();
        var pointOfInterestEntity = pointOfInterest.ToPointOfInterestEntity(city.Id);
        _pointOfInterestRepository.GetAsync(pointOfInterest.Id, Arg.Any<IEnumerable<string>>()).Returns(pointOfInterestEntity);

        // Act
        var result = await _sut.GetByIdAsync(pointOfInterest.Id);

        // Assert
        result.Should().BeEquivalentTo(pointOfInterest);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        _pointOfInterestRepository.GetAsync(Arg.Any<Guid>(), Arg.Any<IEnumerable<string>>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnPointsOfInterest_WhenAtLeastOnePointOfInterestExistsForCity()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var pointOfInterestList = city.PointsOfInterest;
        var pointOfInterestEntityList = pointOfInterestList.Select(x => x.ToPointOfInterestEntity(city.Id));
        _cityRepository.ExistsAsync(Arg.Any<Guid>()).Returns(true);
        // Mocking specific predicates/expressions isn't that easy in NSubstitue, will have to look more into it
        // Ideally want to mock: _pointOfInterestRepository.GetAsync(x => x.CityId == city.Id, Arg.Any<IEnumerable<string>?>()).Returns(pointOfInterestEntityList);
        // but that returns null because comparing equality on linq isn't easy. One suggestion from a maintainer is to extract your expressions to a public static class and use them in both the service and test
        _pointOfInterestRepository.GetAsync(Arg.Any<Expression<Func<PointOfInterestEntity, bool>>?>(), Arg.Any<IEnumerable<string>?>()).Returns(pointOfInterestEntityList);

        // Act
        var result = await _sut.GetAllAsync(city.Id);

        // Assert
        result.Should().BeEquivalentTo(pointOfInterestList);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPointOfInterestExistsForCity()
    {
        // Arrange
        _cityRepository.ExistsAsync(Arg.Any<Guid>()).Returns(true);
        _pointOfInterestRepository.GetAsync(Arg.Any<Expression<Func<PointOfInterestEntity, bool>>?>(), Arg.Any<IEnumerable<string>?>()).Returns(Enumerable.Empty<PointOfInterestEntity>());

        // Act
        var result = await _sut.GetAllAsync(Guid.NewGuid());

        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldThrowValidationException_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityRepository.ExistsAsync(city.Id).Returns(false);

        // Act
        var action = async () => await _sut.GetAllAsync(city.Id);

        // Assert
        await action.Should().ThrowAsync<ValidationException>().WithMessage($"City with id {city.Id} does not exist");
    }
    
}
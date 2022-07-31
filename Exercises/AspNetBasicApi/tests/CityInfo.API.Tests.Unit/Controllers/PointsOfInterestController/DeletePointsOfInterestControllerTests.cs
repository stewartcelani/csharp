using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CityInfo.API.Controllers;
using CityInfo.API.Exceptions;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers.PointsOfInterestControllerTests;

[ExcludeFromCodeCoverage]
public class DeletePointsOfInterestControllerTests
{
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly IPointOfInterestService _pointOfInterestService = Substitute.For<IPointOfInterestService>();
    private readonly PointsOfInterestController _sut;

    public DeletePointsOfInterestControllerTests()
    {
        _sut = new PointsOfInterestController(_pointOfInterestService, _cityService);
    }

    [Fact]
    public async Task DeletePointOfInterest_ShouldDeletePointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _pointOfInterestService.ExistsAsync(id).Returns(true);
        _pointOfInterestService.DeleteAsync(id).Returns(true);

        // Act
        var result = (NoContentResult)await _sut.DeletePointOfInterest(id);

        // Assert
        result.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeletePointOfInterest_ShouldThrowApiException_WhenThereIsAnErrorDeletingPointOfInterest()
    {
        // Arrange
        var id = Guid.NewGuid();
        _pointOfInterestService.ExistsAsync(id).Returns(true);
        _pointOfInterestService.DeleteAsync(id).Returns(false);

        // Act
        var action = async () => await _sut.DeletePointOfInterest(id);

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage($"Error deleting point of interest with id {id}");
    }

    [Fact]
    public async Task DeletePointOfInterest_ReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _pointOfInterestService.ExistsAsync(id).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.DeletePointOfInterest(id);

        // Assert
        result.StatusCode.Should().Be(404);
    }
}
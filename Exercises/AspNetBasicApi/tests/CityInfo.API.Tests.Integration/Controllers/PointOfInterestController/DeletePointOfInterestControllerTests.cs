using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.v1;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.PointOfInterestController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class DeletePointOfInterestControllerTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly IPointOfInterestService _pointOfInterestService;
    private readonly Faker<City> _cityGenerator;

    public DeletePointOfInterestControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _pointOfInterestService = _serviceScope.ServiceProvider.GetRequiredService<IPointOfInterestService>();
        _cityGenerator = testContext.CityGenerator;
    }

    [Fact]
    public async Task DeletePointOfInterest_ShouldDeletePointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(DeletePointOfInterest_ShouldDeletePointOfInterest_WhenPointOfInterestExists)}");
        var pointOfInterest = city.PointsOfInterest.First();

        // Act
        var preDeleteLookup = await _pointOfInterestService.GetByIdAsync(pointOfInterest.Id);
        var response = await _httpClient.DeleteAsync(ApiRoutesV1.PointsOfInterest.Delete.UrlFor(city.Id, pointOfInterest.Id));
        var postDeleteLookup = await _pointOfInterestService.GetByIdAsync(pointOfInterest.Id);
        
        // Assert
        preDeleteLookup.Should().BeEquivalentTo(pointOfInterest);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        postDeleteLookup.Should().BeNull();
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task DeletePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(DeletePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist)}");
        
        // Act
        var response = await _httpClient.DeleteAsync(ApiRoutesV1.PointsOfInterest.Delete.UrlFor(city.Id, Guid.NewGuid()));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }


    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
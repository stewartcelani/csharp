using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.PointOfInterestController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class GetPointOfInterestTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;

    public GetPointOfInterestTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
    }

    [Fact]
    public async Task GetPointsOfInterest_ShouldReturnPointsOfInterest_WhenPointsOfInterestExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(GetPointsOfInterest_ShouldReturnPointsOfInterest_WhenPointsOfInterestExist)}");
        var expectedPointsOfInterestResponse = city.PointsOfInterest.Select(x => x.ToPointOfInterestResponse());

        // Act
        var response = await _httpClient.GetAsync($"api/cities/{city.Id}/pointsofinterest");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pointsOfInterestResponse = await response.Content.ReadFromJsonAsync<IEnumerable<PointOfInterestResponse>>();
        pointsOfInterestResponse.Should().BeEquivalentTo(expectedPointsOfInterestResponse);
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task GetPointsOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"api/cities/{Guid.NewGuid()}/pointsofinterest");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPointOfInterest_ShouldReturnPointOfInterest_WhenPointOfInterestExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(GetPointOfInterest_ShouldReturnPointOfInterest_WhenPointOfInterestExists)}");
        var pointOfInterest = city.PointsOfInterest.First();
        var expectedPointOfInterestResponse = pointOfInterest.ToPointOfInterestResponse();

        // Act
        var response = await _httpClient.GetAsync($"api/cities/{city.Id}/pointsofinterest/{pointOfInterest.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pointOfInterestResponse = await response.Content.ReadFromJsonAsync<PointOfInterestResponse>();
        pointOfInterestResponse.Should().BeEquivalentTo(expectedPointOfInterestResponse);

        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task GetPointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(GetPointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist)}");

        // Act
        var response = await _httpClient.GetAsync($"api/cities/{city.Id}/pointsofinterest/{Guid.NewGuid()}");
        
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
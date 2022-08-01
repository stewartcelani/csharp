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
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.CityController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class GetCityControllerTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;

    public GetCityControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
    }

    [Fact]
    public async Task GetCities_ReturnsCities_WhenCitiesExist()
    {
        // Arrange
        await DeleteAllCities();
        var cities = _cityGenerator.Generate(new Random().Next(2, 10));
        var expectedCitiesResponse = cities.Select(x => x.ToExtendedCityResponse());
        foreach (var city in cities)
        {
            await _cityService.CreateAsync(city);
        }
        
        // Act
        var response = await _httpClient.GetAsync("api/cities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var citiesResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ExtendedCityResponse>>();
        citiesResponse!.Should().BeEquivalentTo(expectedCitiesResponse);
        
        // Cleanup
        foreach (var city in cities)
        {
            await _cityService.DeleteAsync(city.Id);
        }
    }
    
    [Fact]
    public async Task GetCities_ReturnsEmptyList_WhenNoCitiesExist()
    {
        // Arrange
        await DeleteAllCities();

        // Act
        var response = await _httpClient.GetAsync("api/cities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var citiesResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ExtendedCityResponse>>();
        citiesResponse!.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCity_ReturnsCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var expectedCityResponse = city.ToExtendedCityResponse();
        await _cityService.CreateAsync(city);

        // Act
        var response = await _httpClient.GetAsync($"api/cities/{city.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cityResponse = await response.Content.ReadFromJsonAsync<ExtendedCityResponse>();
        cityResponse.Should().BeEquivalentTo(expectedCityResponse);
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task GetCity_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"api/cities/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    private async Task DeleteAllCities()
    {
        var cities = (await _cityService.GetAllAsync()).ToList();
        foreach (var city in cities)
        {
            await _cityService.DeleteAsync(city.Id);
        }
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
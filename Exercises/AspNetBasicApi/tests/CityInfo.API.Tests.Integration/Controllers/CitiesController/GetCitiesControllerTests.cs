using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.CitiesController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class GetCitiesControllerTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;

    public GetCitiesControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
    }

    [Fact]
    public async Task GetCities_ReturnsEmptyList_WhenNoCitiesExist()
    {
        // Arrange
        var cities = (await _cityService.GetAllAsync()).ToList();
        foreach (var city in cities)
        {
            await _cityService.DeleteAsync(city.Id);
        }
        
        // Act
        var response = await _httpClient.GetAsync("api/cities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var citiesResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ExtendedCityResponse>>();
        citiesResponse!.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCities_ReturnsCities_WhenCitiesExist()
    {
        // Arrange
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

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
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

namespace CityInfo.API.Tests.Integration.Controllers.CityController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class UpdateCityControllerTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;
    private readonly Faker<CreateCityRequest> _createCityRequestGenerator;

    public UpdateCityControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
        _createCityRequestGenerator = testContext.CityRequestGenerator;

    }

    [Fact]
    public async Task UpdateCity_ShouldUpdateCity_WhenRequestIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        await _cityService.CreateAsync(city);
        var createCityRequest = _createCityRequestGenerator.Generate();
        var updateCityRequest = new UpdateCityRequest
        {
            Id = city.Id,
            CreateCityRequest = createCityRequest
        };
        var expectedCityResponse = updateCityRequest.ToCity().ToCityResponse();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/cities/{city.Id}", createCityRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cityResponse = await response.Content.ReadFromJsonAsync<CityResponse>();
        cityResponse.Should().BeEquivalentTo(expectedCityResponse);
    }

    [Fact]
    public async Task UpdateCity_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var createCityRequest = _createCityRequestGenerator.Generate();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/cities/{Guid.NewGuid()}", createCityRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
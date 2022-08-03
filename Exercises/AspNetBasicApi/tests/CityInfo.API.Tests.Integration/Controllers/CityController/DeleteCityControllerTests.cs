using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.v1;
using CityInfo.API.Domain;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.CityController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class DeleteCityControllerTests: IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;

    public DeleteCityControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
    }

    [Fact]
    public async Task DeleteCity_ShouldDeleteCity_WhenCityExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(DeleteCity_ShouldDeleteCity_WhenCityExists)}");

        // Act
        var response = await _httpClient.DeleteAsync(ApiRoutesV1.Cities.Delete.UrlFor(city.Id));
        var lookup = await _cityService.GetByIdAsync(city.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        lookup.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Act
        var response = await _httpClient.DeleteAsync(ApiRoutesV1.Cities.Delete.UrlFor(Guid.NewGuid()));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
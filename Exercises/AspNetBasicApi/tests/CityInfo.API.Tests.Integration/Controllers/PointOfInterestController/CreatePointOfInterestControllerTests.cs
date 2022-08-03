using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.v1;
using CityInfo.API.Contracts.v1.Requests;
using CityInfo.API.Contracts.v1.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.PointOfInterestController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class CreatePointOfInterestControllerTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;
    private readonly Faker<CreatePointOfInterestRequest> _createPointOfInterestRequestGenerator;

    public CreatePointOfInterestControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
        _createPointOfInterestRequestGenerator = testContext.CreatePointOfInterestRequestGenerator;
    }

    [Fact]
    public async Task CreatePointOfInterest_ShouldCreatePointOfInterest_WhenRequestIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(CreatePointOfInterest_ShouldCreatePointOfInterest_WhenRequestIsValid)}");
        var createPointOfInterestRequest = _createPointOfInterestRequestGenerator.Generate();

        // Act
        var response =
            await _httpClient.PostAsJsonAsync(ApiRoutesV1.PointsOfInterest.Create.UrlFor(city.Id),
                createPointOfInterestRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var pointOfInterestResponse = await response.Content.ReadFromJsonAsync<PointOfInterestResponse>();
        pointOfInterestResponse.Should().BeEquivalentTo(createPointOfInterestRequest);
        response.Headers.Location!.ToString().Should().Be(_httpClient.BaseAddress +
                                                          ApiRoutesV1.PointsOfInterest.Get.UrlFor(city.Id,
                                                              pointOfInterestResponse!.Id));

        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task CreatePointOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var createPointOfInterestRequest = _createPointOfInterestRequestGenerator.Generate();

        // Act
        var response =
            await _httpClient.PostAsJsonAsync(ApiRoutesV1.PointsOfInterest.Create.UrlFor(Guid.NewGuid()),
                createPointOfInterestRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
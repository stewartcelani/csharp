using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.v1;
using CityInfo.API.Contracts.v1.Requests;
using CityInfo.API.Contracts.v1.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.PointOfInterestController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class UpdatePointOfInterestControllerTests : IClassFixture<CityInfoApiFactory>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScope _serviceScope;
    private readonly ICityService _cityService;
    private readonly Faker<City> _cityGenerator;
    private readonly Faker<CreatePointOfInterestRequest> _createPointOfInterestRequestGenerator;

    public UpdatePointOfInterestControllerTests(SharedTestContext testContext, CityInfoApiFactory cityInfoApiFactory)
    {
        _httpClient = testContext.HttpClient;
        _serviceScope = cityInfoApiFactory.Services.CreateScope();
        _cityService = _serviceScope.ServiceProvider.GetRequiredService<ICityService>();
        _cityGenerator = testContext.CityGenerator;
        _createPointOfInterestRequestGenerator = testContext.CreatePointOfInterestRequestGenerator;
    }

    [Fact]
    public async Task UpdatePointOfInterest_ShouldUpdatePointOfInterest_WhenRequestIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(UpdatePointOfInterest_ShouldUpdatePointOfInterest_WhenRequestIsValid)}");
        var pointOfInterest = city.PointsOfInterest.First();
        var createPointOfInterestRequest = _createPointOfInterestRequestGenerator.Generate();

        // Act
        var response =
            await _httpClient.PutAsJsonAsync(ApiRoutesV1.PointsOfInterest.Update.UrlFor(city.Id, pointOfInterest.Id), createPointOfInterestRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pointOfInterestResponse = await response.Content.ReadFromJsonAsync<PointOfInterestResponse>();
        pointOfInterestResponse.Should().BeEquivalentTo(createPointOfInterestRequest);
        pointOfInterestResponse!.Id.Should().Be(pointOfInterest.Id);
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task UpdatePointOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var createPointOfInterestRequest = _createPointOfInterestRequestGenerator.Generate();

        // Act
        var response =
            await _httpClient.PutAsJsonAsync(ApiRoutesV1.PointsOfInterest.Update.UrlFor(Guid.NewGuid(), Guid.NewGuid()),
                createPointOfInterestRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(UpdatePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist)}");
        var createPointOfInterestRequest = _createPointOfInterestRequestGenerator.Generate();
        
        // Act
        var response =
            await _httpClient.PutAsJsonAsync(ApiRoutesV1.PointsOfInterest.Update.UrlFor(city.Id, Guid.NewGuid()),
                createPointOfInterestRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task PartiallyUpdatePointOfInterest_ShouldPatchPointOfInterest_WhenRequestIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(PartiallyUpdatePointOfInterest_ShouldPatchPointOfInterest_WhenRequestIsValid)}");
        var pointOfInterest = city.PointsOfInterest.First();
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        var existingName = pointOfInterest.Name;
        const string updatedName = "New Name";
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", existingName, updatedName));
        var serializedDoc = JsonConvert.SerializeObject(patchDocument);
        var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
        
        // Act
        var response = await _httpClient.PatchAsync(ApiRoutesV1.PointsOfInterest.Update.UrlFor(city.Id, pointOfInterest.Id),
            requestContent);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pointOfInterestResponse = await response.Content.ReadFromJsonAsync<PointOfInterestResponse>();
        pointOfInterestResponse!.Id.Should().Be(pointOfInterest.Id);
        pointOfInterestResponse!.Name.Should().Be(updatedName);
        pointOfInterestResponse!.Description.Should().Be(pointOfInterest.Description);
        
        // Cleanup
        await _cityService.DeleteAsync(city.Id);
    }

    [Fact]
    public async Task PartiallyUpdatePointOfInterest_ShouldReturnNotFound_WhenCityDoesNotExist()
    {
        // Arrange
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", "Old Name", "New Name"));
        var serializedDoc = JsonConvert.SerializeObject(patchDocument);
        var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
        
        // Act
        var response = await _httpClient.PatchAsync(ApiRoutesV1.PointsOfInterest.Update.UrlFor(Guid.NewGuid(), Guid.NewGuid()),
            requestContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task PartiallyUpdatePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var created = await _cityService.CreateAsync(city);
        if (!created)
            throw new Exception(
                $"City with id {city.Id} could not be created to test {nameof(PartiallyUpdatePointOfInterest_ShouldReturnNotFound_WhenPointOfInterestDoesNotExist)}");
        var patchDocument = new JsonPatchDocument<CreatePointOfInterestRequest>();
        patchDocument.Operations.Add(
            new Operation<CreatePointOfInterestRequest>("replace", "/name", "Old Name", "New Name"));
        var serializedDoc = JsonConvert.SerializeObject(patchDocument);
        var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
        
        // Act
        var response = await _httpClient.PatchAsync(ApiRoutesV1.PointsOfInterest.Update.UrlFor(city.Id, Guid.NewGuid()),
            requestContent);

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
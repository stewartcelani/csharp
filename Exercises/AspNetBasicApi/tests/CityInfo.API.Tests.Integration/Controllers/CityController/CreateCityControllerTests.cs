using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.v1;
using CityInfo.API.Contracts.v1.Requests;
using CityInfo.API.Contracts.v1.Responses;
using FluentAssertions;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.CityController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class CreateCityControllerTests
{
    private readonly HttpClient _httpClient;

    private readonly Faker<CreateCityRequest> _createCityRequestGenerator;

    public CreateCityControllerTests(SharedTestContext testContext)
    {
        _httpClient = testContext.HttpClient;
        _createCityRequestGenerator = testContext.CityRequestGenerator;
    }

    [Fact]
    public async Task CreateCity_ShouldCreateCity_WhenRequestIsValid()
    {
        // Arrange
        var createCityRequest = _createCityRequestGenerator.Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync(ApiRoutesV1.Cities.Create.Url, createCityRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var cityResponse = await response.Content.ReadFromJsonAsync<CityResponse>();
        cityResponse.Should().BeEquivalentTo(createCityRequest);
        response.Headers.Location!.ToString().Should().Be(_httpClient.BaseAddress + ApiRoutesV1.Cities.Get.UrlFor(cityResponse!.Id));
    }
}
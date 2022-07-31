using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Contracts.Responses;
using FluentAssertions;
using Xunit;

namespace CityInfo.API.Tests.Integration.Controllers.CitiesController;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class CreateCitiesControllerTests
{
    private readonly HttpClient _httpClient;

    private readonly Faker<CreateCityRequest> _createCityRequestGenerator;

    public CreateCitiesControllerTests(SharedTestContext testContext)
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
        var response = await _httpClient.PostAsJsonAsync("api/cities", createCityRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var cityResponse = await response.Content.ReadFromJsonAsync<CityResponse>();
        cityResponse.Should().BeEquivalentTo(createCityRequest);
        response.Headers.Location!.ToString().Should().EndWith($"/api/cities/{cityResponse!.Id}");
    }

}
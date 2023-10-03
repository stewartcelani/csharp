using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Xunit;

namespace Customers.Api.Tests.Integration.CustomerController;

[ExcludeFromCodeCoverage]
public class GetCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<CustomerRequest> _customerRequestGenerator;

    public GetCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _httpClient = customerApiFactory.CreateClient();
        _customerRequestGenerator = customerApiFactory.CustomerRequestGenerator;
    }

    [Fact]
    public async Task Get_ReturnsCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerRequest = _customerRequestGenerator.Generate();
        var createResponse = await _httpClient.PostAsJsonAsync("customers", customerRequest);
        var createCustomerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var getResponse = await _httpClient.GetAsync($"customers/{createCustomerResponse!.Id}");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var getCustomerResponse = await getResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        getCustomerResponse.Should().BeEquivalentTo(createCustomerResponse);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentCustomerId = Guid.NewGuid();
        
        // Act
        var response = await _httpClient.GetAsync($"customers/{nonExistentCustomerId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}
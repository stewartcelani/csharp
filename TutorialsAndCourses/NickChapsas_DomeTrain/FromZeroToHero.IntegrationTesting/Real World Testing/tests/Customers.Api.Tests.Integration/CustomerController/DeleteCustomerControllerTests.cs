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
public class DeleteCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<CustomerRequest> _customerRequestGenerator;

    public DeleteCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _httpClient = customerApiFactory.CreateClient();
        _customerRequestGenerator = customerApiFactory.CustomerRequestGenerator;
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var customerRequest = _customerRequestGenerator.Generate();
        var createResponse = await _httpClient.PostAsJsonAsync("customers", customerRequest);
        var customerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var deleteResponse = await _httpClient.DeleteAsync($"customers/{customerResponse!.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Act
        var deleteResponse = await _httpClient.DeleteAsync($"customers/{Guid.NewGuid()}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    

    
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
public class GetAllCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly CustomerApiFactory _customerApiFactory;
    private readonly HttpClient _httpClient;
    private readonly Faker<CustomerRequest> _customerRequestGenerator;

    public GetAllCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _customerApiFactory = customerApiFactory;
        _httpClient = customerApiFactory.CreateClient();
        _customerRequestGenerator = customerApiFactory.CustomerRequestGenerator;
    }

    [Fact]
    public async Task GetAll_ReturnsCustomers_WhenCustomersExists()
    {
        // Arrange
        var customerRequest = _customerRequestGenerator.Generate();
        var createResponse = await _httpClient.PostAsJsonAsync("customers", customerRequest);
        var createCustomerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var getAllResponse = await _httpClient.GetAsync($"customers");

        // Assert
        getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var getAllCustomersResponse = await getAllResponse.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        getAllCustomersResponse!.Customers.Single().Should().BeEquivalentTo(createCustomerResponse);
        
        // Cleanup for GetAll_ReturnsEmptyResult_WhenNoCustomersExist
        await _httpClient.DeleteAsync($"customers/{createCustomerResponse!.Id}");
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyResult_WhenNoCustomersExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"customers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var getAllCustomersResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        getAllCustomersResponse!.Customers.Should().BeEmpty();
    }

}
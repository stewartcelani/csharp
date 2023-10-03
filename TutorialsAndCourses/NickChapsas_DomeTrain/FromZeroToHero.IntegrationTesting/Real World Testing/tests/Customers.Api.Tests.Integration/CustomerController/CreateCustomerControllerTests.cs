using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Customers.Api.Tests.Integration.CustomerController;

[ExcludeFromCodeCoverage]
public class CreateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<CustomerRequest> _customerRequestGenerator;

    public CreateCustomerControllerTests(CustomerApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _customerRequestGenerator = apiFactory.CustomerRequestGenerator;
    }

    [Fact]
    public async Task Create_CreatesUser_WhenDataIsValid()
    {
        // Arrange
        var customer = _customerRequestGenerator.Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("customers", customer);
        
        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.Should().BeEquivalentTo(customer);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be($"http://localhost/customers/{customerResponse!.Id}");
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = nameof(invalidEmail);
        var customer = _customerRequestGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("customers", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
    }
    
    [Fact]
    public async Task Create_ReturnsValidationError_WhenGitHubUsernameIsInvalid()
    {
        // Arrange
        const string invalidGitHubUsername = nameof(invalidGitHubUsername);
        var customer = _customerRequestGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, invalidGitHubUsername).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("customers", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["GitHubUsername"][0].Should().Be($"There is no GitHub user with username {invalidGitHubUsername}");
    }

    [Fact]
    public async Task Create_ReturnsInternalServerError_WhenGitHubIsThrottled()
    {
        // Arrange
        var customer = _customerRequestGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ThrottledGitHubUser)
            .Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("customers", customer);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
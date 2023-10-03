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
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Customers.Api.Tests.Integration.CustomerController;

[ExcludeFromCodeCoverage]
public class UpdateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly Faker<CustomerRequest> _customerRequestGenerator;

    public UpdateCustomerControllerTests(CustomerApiFactory customerApiFactory)
    {
        _httpClient = customerApiFactory.CreateClient();
        _customerRequestGenerator = customerApiFactory.CustomerRequestGenerator;
    }

    [Fact]
    public async Task Update_UpdatesUser_WhenDataIsValid()
    {
        // Arrange
        var customerRequest = _customerRequestGenerator.Generate();
        var createResponse = await _httpClient.PostAsJsonAsync("customers", customerRequest);
        var customerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        var customerRequestUpdated = _customerRequestGenerator.Generate();

        // Act
        var updateResponse =
            await _httpClient.PutAsJsonAsync($"customers/{customerResponse!.Id}", customerRequestUpdated);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updateCustomerResponse = await updateResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        updateCustomerResponse.Should().BeEquivalentTo(customerRequestUpdated);
        updateCustomerResponse!.Id.Should().Be(customerResponse.Id);
    }
    
    [Fact]
    public async Task Update_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = nameof(invalidEmail);
        var customerRequest = _customerRequestGenerator.Generate();
        var createResponse = await _httpClient.PostAsJsonAsync("customers", customerRequest);
        var customerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        var customerRequestUpdated = _customerRequestGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var updateResponse =
            await _httpClient.PutAsJsonAsync($"customers/{customerResponse!.Id}", customerRequestUpdated);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await updateResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
    }
    
    [Fact]
    public async Task Update_ReturnsValidationError_WhenGitHubUserDoesNotExist()
    {
        // Arrange
        const string invalidGitHubUsername = nameof(invalidGitHubUsername);
        var customerRequest = _customerRequestGenerator.Generate();
        var createResponse = await _httpClient.PostAsJsonAsync("customers", customerRequest);
        var customerResponse = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        var customerRequestUpdated = _customerRequestGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, invalidGitHubUsername).Generate();

        // Act
        var updateResponse =
            await _httpClient.PutAsJsonAsync($"customers/{customerResponse!.Id}", customerRequestUpdated);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await updateResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["GitHubUsername"][0].Should().Be($"There is no GitHub user with username {invalidGitHubUsername}");
    }
    
}
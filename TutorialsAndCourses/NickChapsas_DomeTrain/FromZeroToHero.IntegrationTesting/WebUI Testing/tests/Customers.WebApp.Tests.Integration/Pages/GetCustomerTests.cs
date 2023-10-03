using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Customers.WebApp.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Xunit;

namespace Customers.WebApp.Tests.Integration.Pages;

[ExcludeFromCodeCoverage]
[Collection(nameof(SharedTestCollection))]
public class GetCustomerTests : IClassFixture<CustomerWebAppFactory>
{
    private readonly SharedTestContext _testContext;
    private readonly ICustomerService _customerService;

    public GetCustomerTests(SharedTestContext testContext, CustomerWebAppFactory customerWebAppFactory)
    {
        _testContext = testContext;
        _customerService = customerWebAppFactory.Services.GetRequiredService<ICustomerService>();
    }

    [Fact]
    public async Task Get_ReturnsCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = _testContext.CustomerGenerator.Generate();
        await _customerService.CreateAsync(customer);
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AppUrl
        });

        // Act
        await page.GotoAsync($"customer/{customer.Id}");

        // Assert
        (await page.Locator("p[id=fullname-field]").InnerTextAsync()).Should().Be(customer.FullName);
        (await page.Locator("p[id=email-field]").InnerTextAsync()).Should().Be(customer.Email);
        (await page.Locator("p[id=github-username-field]").InnerTextAsync()).Should().Be(customer.GitHubUsername);
        (await page.Locator("p[id=dob-field]").InnerTextAsync()).Should().Be(customer.DateOfBirth.ToString("dd/MM/yyyy"));
    }

    [Fact]
    public async Task Get_ReturnsNoCustomer_WhenNoCustomerExists()
    {
        // Arrange
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AppUrl
        });

        // Act
        await page.GotoAsync($"customer/{Guid.NewGuid()}");

        // Assert
        var element = page.Locator("article>p").First;
        var text = await element.InnerTextAsync();
        text.Should().Be("No customer found with this id");
    }
}
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
public class GetAllCustomerTests : IClassFixture<CustomerWebAppFactory>
{
    private readonly SharedTestContext _testContext;
    private readonly ICustomerService _customerService;

    public GetAllCustomerTests(SharedTestContext testContext, CustomerWebAppFactory customerWebAppFactory)
    {
        _testContext = testContext;
        _customerService = customerWebAppFactory.Services.GetRequiredService<ICustomerService>();
    }

    [Fact]
    public async Task GetAll_ContainsCustomer_WhenCustomerExists()
    {
        // Arrange
        var customers = await _customerService.GetAllAsync();
        foreach (var c in customers)
        {
            await _customerService.DeleteAsync(c.Id);
        }
        var customer = _testContext.CustomerGenerator.Generate();
        await _customerService.CreateAsync(customer);
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AppUrl
        });

        // Act
        await page.GotoAsync($"customers");

        // Assert
        var element = page.Locator("table>tbody>tr").First;
        (await element.Locator("td >> nth=0").InnerTextAsync()).Should().Be(customer.FullName);
        (await element.Locator("td >> nth=1").InnerTextAsync()).Should().Be(customer.Email);
        (await element.Locator("td >> nth=2").InnerTextAsync()).Should().Be(customer.GitHubUsername);
        (await element.Locator("td >> nth=3").InnerTextAsync()).Should().Be(customer.DateOfBirth.ToString("dd/MM/yyyy"));
        await page.CloseAsync();
    }
    
}
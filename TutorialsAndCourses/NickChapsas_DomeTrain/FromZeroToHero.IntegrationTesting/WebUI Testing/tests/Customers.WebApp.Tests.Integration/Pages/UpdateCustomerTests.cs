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
public class UpdateCustomerTests : IClassFixture<CustomerWebAppFactory>
{
    private readonly SharedTestContext _testContext;
    private readonly ICustomerService _customerService;

    public UpdateCustomerTests(SharedTestContext testContext, CustomerWebAppFactory customerWebAppFactory)
    {
        _testContext = testContext;
        _customerService = customerWebAppFactory.Services.GetRequiredService<ICustomerService>();
    }
    
    [Fact]
    public async Task Update_UpdatesCustomer_WhenDataIsValid()
    {
        // Arrange
        var customer = _testContext.CustomerGenerator.Generate();
        await _customerService.CreateAsync(customer);
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AppUrl
        });
        var updatedCustomer = _testContext.CustomerGenerator.Generate();
        
        // Act
        await page.GotoAsync($"update-customer/{customer.Id}");
        await page.FillAsync("input[id=fullname]", updatedCustomer.FullName);
        await page.FillAsync("input[id=email]", updatedCustomer.Email);
        await page.FillAsync("input[id=github-username]", updatedCustomer.GitHubUsername);
        await page.FillAsync("input[id=dob]", updatedCustomer.DateOfBirth.ToString("yyyy-MM-dd"));
        await page.ClickAsync("button[type=submit]");

        // Assert
        var linkElement = page.Locator("article>p>a").First;
        var link = await linkElement.GetAttributeAsync("href");
        await page.GotoAsync(link!);
        (await page.Locator("p[id=fullname-field]").InnerTextAsync()).Should().Be(updatedCustomer.FullName);
        (await page.Locator("p[id=email-field]").InnerTextAsync()).Should().Be(updatedCustomer.Email);
        (await page.Locator("p[id=github-username-field]").InnerTextAsync()).Should().Be(updatedCustomer.GitHubUsername);
        (await page.Locator("p[id=dob-field]").InnerTextAsync()).Should().Be(updatedCustomer.DateOfBirth.ToString("dd/MM/yyyy"));
        await page.CloseAsync();
    }
    
    [Fact]
    public async Task Update_ShowsError_WhenEmailIsInvalid()
    {
        // Arrange
        var customer = _testContext.CustomerGenerator.Generate();
        await _customerService.CreateAsync(customer);
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AppUrl
        });
        var updatedCustomer = _testContext.CustomerGenerator.Generate();
        
        // Act
        await page.GotoAsync($"update-customer/{customer.Id}");
        await page.FillAsync("input[id=fullname]", updatedCustomer.FullName);
        await page.FillAsync("input[id=email]", "invalidemail");
        await page.FillAsync("input[id=github-username]", updatedCustomer.GitHubUsername);
        await page.FillAsync("input[id=dob]", updatedCustomer.DateOfBirth.ToString("yyyy-MM-dd"));

        // Assert
        var element = page.Locator("li.validation-message").First;
        var text = await element.InnerTextAsync();
        text.Should().Be("Invalid email format");
        await page.CloseAsync();
    }
    
    
}
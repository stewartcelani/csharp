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
public class DeleteCustomerTests: IClassFixture<CustomerWebAppFactory>
{
    private readonly SharedTestContext _testContext;
    private readonly ICustomerService _customerService;

    public DeleteCustomerTests(SharedTestContext testContext, CustomerWebAppFactory customerWebAppFactory)
    {
        _testContext = testContext;
        _customerService = customerWebAppFactory.Services.GetRequiredService<ICustomerService>();
    }

    [Fact]
    public async Task Delete_DeletesCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = _testContext.CustomerGenerator.Generate();
        await _customerService.CreateAsync(customer);
        var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
        {
            BaseURL = SharedTestContext.AppUrl
        });
        page.Dialog += (_, dialog) => dialog.AcceptAsync();

        // Act
        await page.GotoAsync($"customer/{customer.Id}");
        await page.ClickAsync("button:has-text('Delete')");

        // Assert
        page.Url.Should().Be(SharedTestContext.AppUrl); // Account for trailing slash
        var customerLookup = await _customerService.GetAsync(customer.Id);
        customerLookup.Should().BeNull();
    }
    
}
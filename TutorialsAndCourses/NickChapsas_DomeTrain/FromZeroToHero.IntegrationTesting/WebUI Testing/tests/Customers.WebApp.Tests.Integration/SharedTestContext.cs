using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Customers.WebApp.Models;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Microsoft.Playwright;
using Xunit;

namespace Customers.WebApp.Tests.Integration;

[ExcludeFromCodeCoverage]
public class SharedTestContext : IAsyncLifetime
{
    public const string ValidGitHubUsername = "validuser";
    public const string ThrottledGitHubUser = "throttleduser";
    public const string AppUrl = "https://localhost:7780/";
    

    public readonly Faker<Customer> CustomerGenerator = new Faker<Customer>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => DateOnly.FromDateTime(faker.Person.DateOfBirth))
        .RuleFor(x => x.GitHubUsername, ValidGitHubUsername);

    public GitHubApiServer GitHubApiServer { get; } = new();

    private static readonly string DockerComposeFile =
        Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"../../../docker-compose.integration.yml");

    private IPlaywright _playwright;
    
    public IBrowser Browser { get; private set; }
    
    private readonly ICompositeService _dockerService = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposeFile)
        .ForceRecreate() // This will ensure the database is recreated every time tests are launched
        .RemoveOrphans()
        .WaitForHttp("test-app", AppUrl)
        .Build();
    
    public async Task InitializeAsync()
    {
        GitHubApiServer.Start();
        GitHubApiServer.SetupUser(ValidGitHubUsername);
        GitHubApiServer.SetupThrottledUser(ThrottledGitHubUser);
        _dockerService.Start();
        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            //Headless = false,
            //SlowMo = 1000
            SlowMo = 150 // Nick says he usually adds a slowmo of 150 because of how browsers work and the way things need to interact with each other (don't want to go TOO fast and have tests break due to that)
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        _playwright.Dispose();
        _dockerService.Dispose();
        GitHubApiServer.Dispose();
    }
}
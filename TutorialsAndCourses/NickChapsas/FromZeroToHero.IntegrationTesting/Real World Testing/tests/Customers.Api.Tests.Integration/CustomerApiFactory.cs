using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Database;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Customers.Api.Tests.Integration;

[ExcludeFromCodeCoverage]
public class CustomerApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    /*private readonly TestcontainersContainer _dbContainer =
        new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("postgres:latest")
            .WithEnvironment("POSTGRES_USER", "course")
            .WithEnvironment("POSTGRES_PASSWORD", "changeme")
            .WithEnvironment("POSTGRES_DB", "mydb")
            .WithPortBinding(5555, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();*/

    public const string ValidGitHubUser = "validuser";
    public const string ThrottledGitHubUser = "throttleduser";
    
    public readonly Faker<CustomerRequest> CustomerRequestGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
        .RuleFor(x => x.GitHubUsername, ValidGitHubUser);

    
    // Testcontainers has first-party support for Postgres but for things without first-party support the above code works
    // DB will run on a random port and the connection string is accessible on _dbContainer.ConnectionString
    private readonly TestcontainerDatabase _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "course",
            Password = "whatever"
        }).Build();

    private readonly GitHubApiServer _gitHubApiServer = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(_dbContainer.ConnectionString));
            
            services.AddHttpClient("GitHub", httpClient =>
            {
                httpClient.BaseAddress = new Uri(_gitHubApiServer.Url);
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/vnd.github.v3+json");
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
            });
        });
    }

    public async Task InitializeAsync()
    {
        _gitHubApiServer.Start();
        await _dbContainer.StartAsync();
        _gitHubApiServer.SetupUser(ValidGitHubUser);
        _gitHubApiServer.SetupThrottledUser(ThrottledGitHubUser);
    }

    public new async Task DisposeAsync()
    {
        _gitHubApiServer.Dispose();
        await _dbContainer.DisposeAsync();
    }
}
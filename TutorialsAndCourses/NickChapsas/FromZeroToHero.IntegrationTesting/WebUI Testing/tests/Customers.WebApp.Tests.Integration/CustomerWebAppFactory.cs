using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Customers.WebApp.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Customers.WebApp.Tests.Integration;

[ExcludeFromCodeCoverage]
public class CustomerWebAppFactory : WebApplicationFactory<IAppMarker>
{
    private const string DockerHostDatabaseConnectionString = "Server=host.docker.internal;Port=5435;Database=mydb;User ID=course;Password=changeme";
    private const string WireMockGitHubApiBaseUrl = "http://localhost:9850";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IHostedService>();
            services.RemoveAll<IDbConnectionFactory>();
            services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(DockerHostDatabaseConnectionString));
            
            services.AddHttpClient("GitHub", httpClient =>
            {
                httpClient.BaseAddress = new Uri(WireMockGitHubApiBaseUrl);
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/vnd.github.v3+json");
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
            });
        });
    }
}
using System.Diagnostics.CodeAnalysis;
using CityInfo.API.Domain.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Tests.Integration;

[ExcludeFromCodeCoverage]
public class CityInfoApiFactory : WebApplicationFactory<IApiMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DatabaseSettings>();
            var databaseSettings = new DatabaseSettings
            {
                ConnectionString =
                    "Host=host.docker.internal;Username=testuser;Password=testpassword;Database=testdb;Port=5439;",
                EnableSensitiveDataLogging = false,
                SeedData = false
            };
            services.AddSingleton(databaseSettings);
        });
    }
}
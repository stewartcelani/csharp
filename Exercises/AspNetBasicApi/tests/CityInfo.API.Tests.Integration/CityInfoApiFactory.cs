using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Contracts.Requests;
using CityInfo.API.Data;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Settings;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

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
            services.RemoveAll<IHostedService>();
            /*services.RemoveAll<ApplicationDbContext>();*/
            services.RemoveAll<DatabaseSettings>();

            var databaseSettings = new DatabaseSettings
            {
                ConnectionString =
                    "Host=host.docker.internal;Username=testuser;Password=testpassword;Database=testdb;Port=5439;",
                EnableSensitiveDataLogging = false,
                SeedData = false
            };
            services.AddSingleton(databaseSettings);

            /*
            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
                optionsBuilder.UseNpgsql(DockerHostDatabaseConnectionString));*/
        });
    }

}
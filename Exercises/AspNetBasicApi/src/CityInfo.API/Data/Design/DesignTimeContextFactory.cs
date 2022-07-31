using CityInfo.API.Domain.Settings;
using CityInfo.API.Domain.Settings.Helpers;
using CityInfo.API.Logging;
using CityInfo.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Data.Design;

/// <summary>
///     Used by dotnet ef migrations tool
/// </summary>
public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        ILoggerAdapter<ApplicationDbContext> logger =
            new LoggerAdapter<ApplicationDbContext>(new LoggerFactory().CreateLogger<ApplicationDbContext>());

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var databaseSettings =
            SettingsBinder.BindAndValidate<DatabaseSettings, DatabaseSettingsValidator>(configuration);

        builder.UseNpgsql(databaseSettings.ConnectionString);

        return new ApplicationDbContext(builder.Options, databaseSettings, logger);
    }
}
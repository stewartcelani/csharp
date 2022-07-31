using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CityInfo.API.Data.ValueConverters;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Domain.Entities.Common;
using CityInfo.API.Domain.Settings;
using CityInfo.API.Logging;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Data;

public class ApplicationDbContext : DbContext
{
    private readonly DatabaseSettings _databaseSettings;
    private readonly ILoggerAdapter<ApplicationDbContext> _logger;


    public ApplicationDbContext(DatabaseSettings databaseSettings, ILoggerAdapter<ApplicationDbContext> logger)
    {
        _databaseSettings = databaseSettings;
        _logger = logger;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        DatabaseSettings databaseSettings, ILoggerAdapter<ApplicationDbContext> logger) : base(options)
    {
        _databaseSettings = databaseSettings;
        _logger = logger;
    }

    public DbSet<CityEntity> City { get; set; }
    public DbSet<PointOfInterestEntity> PointOfInterest { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_databaseSettings.EnableSensitiveDataLogging)
            optionsBuilder.UseNpgsql(_databaseSettings.ConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure())
                .EnableSensitiveDataLogging()
                .LogTo(s => _logger.LogDebug(s));
        else
            optionsBuilder.UseNpgsql(_databaseSettings.ConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure())
                .LogTo(s => _logger.LogDebug(s));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.Now;

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.DateUpdated = now;
                    entry.Property(x => x.DateCreated).IsModified = false;
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}
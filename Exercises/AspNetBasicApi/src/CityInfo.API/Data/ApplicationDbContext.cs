using System.Reflection;
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


    public ApplicationDbContext(ILoggerAdapter<ApplicationDbContext> logger, DatabaseSettings databaseSettings)
    {
        _logger = logger;
        _databaseSettings = databaseSettings;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ILoggerAdapter<ApplicationDbContext> logger, DatabaseSettings databaseSettings) : base(options)
    {
        _logger = logger;
        _databaseSettings = databaseSettings;
    }

    public DbSet<CityEntity> City { get; set; }
    public DbSet<PointOfInterestEntity> PointOfInterest { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_databaseSettings.EnableSensitiveDataLogging)
            optionsBuilder.UseSqlite(_databaseSettings.ConnectionString)
                .EnableSensitiveDataLogging()
                .LogTo(s => _logger.LogDebug(s));
        else
            optionsBuilder.UseSqlite(_databaseSettings.ConnectionString)
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
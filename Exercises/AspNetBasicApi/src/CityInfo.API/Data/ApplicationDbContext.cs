using System.Reflection;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Domain.Entities.Common;
using CityInfo.API.Logging;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Data;

public class ApplicationDbContext : DbContext
{
    private ILoggerAdapter<ApplicationDbContext> _logger;

    private const string _databaseName = "app.db";
    
    public DbSet<CityEntity> City { get; set; }
    public DbSet<PointOfInterestEntity> PointOfInterest { get; set; }


    public ApplicationDbContext(ILoggerAdapter<ApplicationDbContext> logger)
    {
        _logger = logger;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerAdapter<ApplicationDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_databaseName};")
            .EnableSensitiveDataLogging()
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
        {
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
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
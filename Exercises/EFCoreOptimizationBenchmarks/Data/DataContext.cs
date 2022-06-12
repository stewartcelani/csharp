using EFCoreOptimizationBenchmarks.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreOptimizationBenchmarks.Data;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=mssql;Database=Benchmark;User=sa;Password=Password1!")
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Many to many
        modelBuilder.Entity<UserRole>()
            .HasKey(x => new { x.UserId, x.RoleId });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);
        foreach (var insertedEntry in insertedEntries)
            //If the inserted object is BaseEntity. 
            if (insertedEntry is BaseEntity baseEntity)
                baseEntity.DateCreated = now;

        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        foreach (var modifiedEntry in modifiedEntries)
            //If the inserted object is BaseEntity. 
            if (modifiedEntry is BaseEntity baseEntity)
            {
                baseEntity.DateUpdated = now;
                Entry(baseEntity).Property(x => x.DateCreated).IsModified = false;
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}
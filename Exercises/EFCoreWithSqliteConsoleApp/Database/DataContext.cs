using EFCoreWithSqliteConsoleApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreWithSqliteConsoleApp.Database;

public sealed class DataContext : DbContext
{
    private readonly string _databaseName = Guid.NewGuid() + ".db";
    
    public DataContext()
    {
        Database.EnsureCreated();
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, _databaseName);
    }

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    public string DbPath { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_databaseName};")
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        {
            //If the inserted object is BaseEntity. 
            if (insertedEntry is BaseEntity baseEntity) baseEntity.DateCreated = now;
        }

        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        foreach (var modifiedEntry in modifiedEntries)
        {
            //If the inserted object is BaseEntity. 
            if (modifiedEntry is BaseEntity auditableEntity)
            {
                auditableEntity.DateUpdated = now;
                Entry(auditableEntity).Property(x => x.DateCreated).IsModified = false;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
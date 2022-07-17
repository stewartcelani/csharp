using Microsoft.EntityFrameworkCore;
using WorkingWithGenericInterfaces.Entities;

namespace WorkingWithGenericInterfaces.Data;

public class DataContext : DbContext
{
    public DbSet<Employee> Employees { get; init; }
    public DbSet<Organization> Organizations { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase("WorkingWithGenericInterfaces");
    }

    public override int SaveChanges()
    {
        var now = DateTime.UtcNow;

        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);
        foreach (var insertedEntry in insertedEntries)
            if (insertedEntry is IAuditable auditable)
                auditable.CreatedDate = now;

        var updatedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        foreach (var updatedEntry in updatedEntries)
        {
            if (updatedEntry is not IAuditable auditable) continue;
            auditable.ModifiedDate = now;
            Entry(auditable).Property(x => x.CreatedDate).IsModified = false;
        }

        return base.SaveChanges();
    }
}
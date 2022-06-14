using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using GenericMethodsAndDelegates.Entities;

namespace GenericMethodsAndDelegates.Data;

public class DataContext : DbContext
{
    public DbSet<Employee> Employees { get; init; }
    public DbSet<Organization> Organizations { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }

    public override int SaveChanges()
    {
        /*
         * This was not part of the course
         */
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
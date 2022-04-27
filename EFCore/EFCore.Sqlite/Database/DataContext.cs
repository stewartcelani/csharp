using System.Diagnostics;
using EFCore.Sqlite.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Sqlite.Database
{
    public sealed class DataContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Role> UserRole { get; set; }
        
        public string DbPath { get; set; }

        public DataContext() : base()
        {
            Database.EnsureCreated();
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "EFCore.Sqlite.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=database.db;");
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
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
            var insertedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);
            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Auditable;
                //If the inserted object is an Auditable. 
                if (auditableEntity != null)
                {
                    auditableEntity.DateCreated = now;
                }
            }
            var modifiedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity);
            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Auditable. 
                var auditableEntity = modifiedEntry as Auditable;
                if (auditableEntity != null)
                {
                    auditableEntity.DateUpdated = now;
                    this.Entry(auditableEntity).Property(x => x.DateCreated).IsModified = false;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            var now = DateTimeOffset.UtcNow;
            var insertedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity);
            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as Auditable;
                //If the inserted object is an Auditable. 
                if (auditableEntity != null)
                {
                    auditableEntity.DateCreated = now;
                }
            }
            var modifiedEntries = this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity);
            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Auditable. 
                var auditableEntity = modifiedEntry as Auditable;
                if (auditableEntity != null)
                {
                    auditableEntity.DateUpdated = now;
                    this.Entry(auditableEntity).Property(x => x.DateCreated).IsModified = false;
                }
            }

            return base.SaveChanges();
        }
        
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tweetbook.Data;

namespace Tweetbook.Extensions;

public static class DataExtensions
{
    /*
     * Example:
     * modelBuilder.SetDeleteBehaviorForType(DeleteBehavior.Restrict, typeof(PostTag).FullName);
     *         public virtual IdentityBuilder AddRoles<TRole>() where TRole : class

     */
    public static void SetDeleteBehaviorForType<TEntity>(this ModelBuilder modelBuilder, DeleteBehavior deleteBehavior)
        where TEntity : class
    {
        var mutableEntityType = modelBuilder.Model.GetEntityTypes().First(x => x.Name == typeof(TEntity).FullName);
        foreach (var mutableForeignKey in mutableEntityType.GetForeignKeys())
            mutableForeignKey.DeleteBehavior = deleteBehavior;
    }

    public static async Task RunPendingMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

        if (dataContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory") // IntegrationTests
            if ((await dataContext.Database.GetPendingMigrationsAsync()).Any())
                await dataContext.Database.MigrateAsync();
    }

    public static async Task EnsureRolesAreSeeded(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            var userRole = new IdentityRole("User");
            await roleManager.CreateAsync(userRole);
        }
    }
}
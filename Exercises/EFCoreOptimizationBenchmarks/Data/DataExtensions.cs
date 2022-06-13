using Bogus;
using Bogus.DataSets;
using EFCoreOptimizationBenchmarks.Entities;
using EFCoreOptimizationBenchmarks.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFCoreOptimizationBenchmarks.Data;

public static class DataExtensions
{
    public static async Task RunPendingMigrations(this DataContext dataContext)
    {
        if (dataContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory") // IntegrationTests
            if ((await dataContext.Database.GetPendingMigrationsAsync()).Any())
                await dataContext.Database.MigrateAsync();
    }

    public static async Task EnsureSeededUsersAndRoles(this DataContext dataContext, int usersToSeed = 200,
        ILogger? logger = null)
    {
        if (dataContext.Role.Any(x => x.Name == "User")) return; // Exit if already seeded

        var roles = new List<Role>
        {
            new()
            {
                Name = "User"
            },
            new()
            {
                Name = "Admin"
            }
        };
        var adminRole = roles.First(x => x.Name == "Admin");
        var userRole = roles.First(x => x.Name == "User");

        Random rand = new();
        var users = new List<User>();
        var userRoles = new List<UserRole>();
        for (var i = 1; i <= usersToSeed; i++)
        {
            logger?.LogInformation($"Generating user #{i}");
            User fakeUser = new Faker<User>()
                .RuleFor(u => u.Sex, f => f.PickRandom<Sex>())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((Name.Gender?)u.Sex))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName((Name.Gender?)u.Sex))
                .RuleFor(u => u.Age, f => f.Random.Number(18, 67))
                .RuleFor(u => u.TaxFileNumber, f => f.Random.Replace("###-###-###").OrNull(f, .66f))
                .RuleFor(u => u.DriversLicenseNumber, f => f.Random.Replace("######").OrNull(f, .15f))
                .RuleFor(u => u.Title, f => f.PickRandom(new[] { "Mr", "Mrs", "Miss" }))
                .RuleFor(u => u.Address, f => f.Address.StreetAddress())
                .RuleFor(u => u.City, f => f.Address.City())
                .RuleFor(u => u.State, f => f.PickRandom(new[] { "WA", "NT", "ACT", "TAS", "QLD", "NSW", "VIC", "SA" }))
                .RuleFor(u => u.Postcode, f => f.Address.ZipCode("####"))
                .RuleFor(u => u.Country, _ => "Australia")
                .RuleFor(u => u.HomePhone, f => f.Phone.PhoneNumber("(##) #### ####"))
                .RuleFor(u => u.Mobile, f => f.Phone.PhoneNumber("0#########"))
                .RuleFor(u => u.Email, f => f.Internet.Email());
            users.Add(fakeUser);

            userRoles.Add(new UserRole
            {
                UserId = fakeUser.Id,
                RoleId = userRole.Id
            });

            if (rand.Next(0, 10) == 0)
                userRoles.Add(new UserRole
                {
                    UserId = fakeUser.Id,
                    RoleId = adminRole.Id
                });
        }

        dataContext.AddRange(roles);
        dataContext.AddRange(users);
        dataContext.AddRange(userRoles);
        await dataContext.SaveChangesAsync();
        dataContext.ChangeTracker.Clear();
        users.Clear();
        userRoles.Clear();
        users = null;
        userRoles = null;
        GC.Collect();
    }
}
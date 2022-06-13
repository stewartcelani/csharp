using Bogus;
using Bogus.DataSets;
using EFCoreWithSqliteConsoleApp.Entities;
using EFCoreWithSqliteConsoleApp.Types;

namespace EFCoreWithSqliteConsoleApp.Database;

public static class DataExtensions
{
    public static async Task EnsureSeededUsersAndRoles(this DataContext dataContext)
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

        var users = new List<User>();
        var userRoles = new List<UserRole>();

        Random rand = new();
        for (var i = 1; i <= 200; i++)
        {
            User fakeUser = new Faker<User>()
                .RuleFor(u => u.Sex, f => f.PickRandom<Sex>())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((Name.Gender?)u.Sex))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName((Name.Gender?)u.Sex))
                .RuleFor(u => u.Age, f => f.Random.Number(18, 67))
                .RuleFor(u => u.TaxFileNumber, f => f.Random.Replace("###-###-###").OrNull(f, .66f));
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
        users = null;
        userRoles.Clear();
        userRoles = null;
        GC.Collect();
    }
}
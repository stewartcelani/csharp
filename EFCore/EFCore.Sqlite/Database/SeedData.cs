using Bogus;
using Bogus.DataSets;
using EFCore.Sqlite.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using EFCore.Sqlite.Models;
using EFCore.Sqlite.Types;

namespace EFCore.Sqlite.Database;

public static class SeedData
{
    public static void EnsureUsersAndRoles(DataContext db)
    {
        if (db.Role.Any(x => x.Name == "User")) return; // Exit if already seeded
        
        var roles = new List<Role>
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Admin"
            }
        };

        var users = new List<User>();
        for (var i = 1; i <= 200; i++)
        {
            User fakeUser = new Faker<User>()
                .RuleFor(u => u.Sex, f => f.PickRandom<Sex>())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName((Name.Gender?)u.Sex))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName((Name.Gender?)u.Sex))
                .RuleFor(u => u.Age, f => f.Random.Number(18, 67))
                .RuleFor(u => u.TaxFileNumber, f => f.Random.Replace("###-###-###").OrNull(f, .66f));
            users.Add(fakeUser);
        }

        var userRoles = new List<UserRole>();
        Role adminRole = roles.First(x => x.Name == "Admin");
        Role userRole = roles.First(x => x.Name == "User");
        Random rand = new();
        foreach (User user in users)
        {
            userRoles.Add(new UserRole()
            {
                UserId = user.Id,
                RoleId = userRole.Id
            });

            if (rand.Next(0, 10) == 0)
            {
                userRoles.Add(new UserRole()
                {
                    UserId = user.Id,
                    RoleId = adminRole.Id
                });
            }
        }
        
        db.AddRange(roles);
        db.AddRange(users);
        db.AddRange(userRoles);
        db.SaveChanges();
    }
}
using EFCoreWithSqliteConsoleApp.Database;
using Microsoft.EntityFrameworkCore;

/*
 * This project has minimum setup for User, Role and UserRole many-to-many table
 * The Sqlite database is generated using a random guid so you can be
 * sure every run is reading from a newly seeded database.
 */

var db = new DataContext();

Console.WriteLine(db.Database.GetConnectionString() + "\n");

await db.EnsureSeededUsersAndRoles();

var users = db.User.Include(x => x.UserRoles).ThenInclude(x => x.Role).ToList();
foreach (var user in users)
{
    var roles = string.Join(" & ", user.UserRoles.Select(x => x.Role.Name).ToArray());
    Console.WriteLine($"{user.Id}: {user.FullName}, {user.Sex}, {user.Age}, {roles}");
}

Console.ReadLine();
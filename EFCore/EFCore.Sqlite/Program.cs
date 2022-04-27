using EFCore.Sqlite.Database;

/*
 * This project has minimum setup for User, Role and UserRole many-to-many table
 * The database.db gets copied to /bin so make sure to connect to that database to see changed
 * dotnet ef migrations add InitialCreate
 * dotnet ef database update
 */
 
var db = new DataContext();
SeedData.EnsureUsersAndRoles(db);
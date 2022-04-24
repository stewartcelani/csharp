using Library.Seeders.Files;

var seeder = new FileSeeder(new FileSeederConfiguration());
List<FileInfo> files = seeder.SeedFiles(10);
Console.ReadLine();
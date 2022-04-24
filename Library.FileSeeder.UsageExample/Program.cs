using Library.FileSeeder;

var seeder = new FileSeeder(new FileSeederConfiguration());
List<FileInfo> files = seeder.SeedFiles(10);
foreach (FileInfo fileInfo in files)
{
    Console.WriteLine(fileInfo.FullName);
}
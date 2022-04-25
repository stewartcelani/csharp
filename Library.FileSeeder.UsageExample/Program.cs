using Library.FileSeeder;

var seeder = new FileSeeder(new FileSeederConfiguration()
{
    Path = Path.Combine(Path.GetTempPath(), "tempseedfiles"),
    FileExtensions = new []{ "tmp" }
});
seeder.SeedFiles(1000, 0);


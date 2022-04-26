using Library.FileSeeder;

string path = Path.Combine(Path.GetTempPath(), "tempseedfiles");
var seeder = new FileSeeder(new FileSeederConfiguration()
{
    MaxFileSizeInMb = 5,
    Path = path,
    FileExtensions = new []{ "tmp" }
});
seeder.SeedFiles(1000, 0);
System.Diagnostics.Process.Start("explorer.exe", $"\"{path}\"");


/*
seeder.SeedFiles(1000); // This would seed files of random size between 0 and 5mb
*/
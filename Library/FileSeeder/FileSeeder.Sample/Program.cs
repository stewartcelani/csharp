using System.Diagnostics;
using FileSeeder;

var path = Path.Combine(Path.GetTempPath(), "tempseedfiles");
var seeder = new FileSeeder.FileSeeder(new FileSeederConfiguration
{
    MaxFileSizeInMb = 1,
    Path = path,
    FileExtensions = new[] { "tmp" }
});
seeder.SeedFiles(1000);
Process.Start("explorer.exe", $"\"{path}\"");


/*
seeder.SeedFiles(1000); // This would seed files of random size between 0 and 5mb
*/
using BenchmarkDotNet.Running;
using Benchmarks.Download.SynchronousVsParallel;
using Library.FileSeeder;

var seeder = new FileSeeder(new FileSeederConfiguration()
{
   FileExtensions = new []{ "tmp" },
   MaxFileSizeInMb = 5,
   Path = @"C:\inetpub\wwwroot\wwwroot\temp\downloadfrom\"
});
//seeder.SeedFiles(1000);

//var benchmark = new Benchmark();
//await benchmark.ParallelLoop();

BenchmarkRunner.Run<Benchmark>();
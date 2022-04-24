using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;

namespace Benchmarks.Download.SynchronousVsParallel;

[MemoryDiagnoser()]
public class Benchmark
{
    private readonly HttpClient _http = new ();
    private readonly string _downloadPath = @"C:\inetpub\wwwroot\wwwroot\temp\downloadto";
    private readonly string _baseDownloadUrl = "https://localhost/temp/downloadfrom";
    private readonly List<File> _files = new();

    public Benchmark()
    {
        if (!Directory.Exists(_downloadPath))
        {
            Directory.CreateDirectory(_downloadPath);
        }

        for (var i = 1; i <= 1000; i++)
        {
            _files.Add(new File()
            {
                DownloadPath = @$"{_downloadPath}\{i}.tmp",
                DownloadUrl = @$"{_baseDownloadUrl}/{i}.tmp"
            });
        }
    }
    
    [Benchmark]
    public async Task SynchronousLoop()
    {
        foreach (File file in _files)
        {
            await using Stream stream = await _http.GetStreamAsync(file.DownloadUrl);
            await using FileStream fileStream = new (file.DownloadPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            //Console.WriteLine($"+ {file.DownloadPath}");
        }
    }

    [Benchmark]
    public async Task ParallelLoop()
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 10
        };
        await Parallel.ForEachAsync(_files, parallelOptions, async (file, ct) =>
        {
            await using Stream stream = await _http.GetStreamAsync(file.DownloadUrl, ct);
            await using FileStream fileStream = new(file.DownloadPath, FileMode.Create);
            await stream.CopyToAsync(fileStream, ct);
            //Console.WriteLine($"+ {file.DownloadPath}");
        });
    }
    
}

public record File
{
    public string DownloadUrl { get; set; }
    public string DownloadPath { get; set; }
}
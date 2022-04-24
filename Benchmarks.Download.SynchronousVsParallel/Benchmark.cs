using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Library.FileDownloader;

namespace Benchmarks.Download.SynchronousVsParallel;

/*
 * Did 100 runs for the initial benchmark in README.md
 * setting follow up runs to only do 5 so finish quick
 */
[SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 5)]
[MemoryDiagnoser()]
public class Benchmark
{
    private readonly FileDownloader _fileDownloader = new ();
    private readonly string _downloadPath = @"C:\inetpub\wwwroot\wwwroot\temp\downloadto";
    private readonly string _baseDownloadUrl = "https://localhost/temp/downloadfrom";
    private readonly List<FileDownloaderFile> _files = new();

    public Benchmark()
    {
        if (!Directory.Exists(_downloadPath))
        {
            Directory.CreateDirectory(_downloadPath);
        }

        for (var i = 1; i <= 1000; i++)
        {
            _files.Add(new FileDownloaderFile()
            {
                DownloadPath = @$"{_downloadPath}\{i}.tmp",
                DownloadUrl = @$"{_baseDownloadUrl}/{i}.tmp"
            });
        }
    }
    
    [Benchmark]
    public async Task SynchronousLoop()
    {
        foreach (FileDownloaderFile file in _files)
        {
            await _fileDownloader.DownloadAsync(file.DownloadUrl, file.DownloadPath);
        }
    }

    [Benchmark]
    public async Task ParallelLoop()
    {
        await _fileDownloader.DownloadParallelAsync(_files);
    }
    
}

using BenchmarkDotNet.Attributes;

namespace FolderSize;

[MaxIterationCount(33)]
public class Benchmarks
{
    private const string Path = @"C:\dev";
    private readonly DirectoryInfo _directoryInfo = new(Path);
    
    [Benchmark]
    public long DirSize()
    {
        return _directoryInfo.DirSize();
    }
    
    [Benchmark]
    public long GetDirectorySize()
    {
        return _directoryInfo.GetDirectorySize();
    }

    [Benchmark]
    public long GetDirectorySizeParallel()
    {
        return _directoryInfo.GetDirectorySizeParallel();
    }
}
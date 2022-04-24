using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Library.FileDownloader.Tests;

public class FileDownloaderTests
{
    private readonly FileDownloader _sut = new();
    private readonly FileDownloaderFile _file = new()
    {
        DownloadUrl = @"https://stewartcelani-public.s3.amazonaws.com/samplefiles/1.tmp",
        DownloadPath = Path.Combine(Path.GetTempPath(), "1.tmp")
    };
    private readonly List<FileDownloaderFile> _fileList = new();

    public FileDownloaderTests()
    {
        for (var i = 1; i <= 10; i++)
        {
            _fileList.Add(new FileDownloaderFile()
            {
                DownloadUrl = _file.DownloadUrl,
                DownloadPath = Path.Combine(Path.GetTempPath(), $"{i}.tmp")
            });
        }
    }

    [Fact]
    public async Task DownloadAsync()
    {
        FileInfo file = await _sut.DownloadAsync(_file.DownloadUrl, _file.DownloadPath);
        file.Exists.Should().BeTrue();
    }

    [Fact]
    public async Task DownloadAsyncOverload()
    {
        FileInfo file = await _sut.DownloadAsync(_file);
        file.Exists.Should().BeTrue();
    }

    [Fact]
    public async Task DownloadParallelAsync()
    {
        List<FileInfo> files = await _sut.DownloadParallelAsync(_fileList);
        files.Count.Should().Be(10);
        files.All(x => x.Exists == true).Should().BeTrue();
    }
}
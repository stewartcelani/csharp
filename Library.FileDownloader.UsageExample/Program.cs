using System.Net.Http.Headers;
using Library.FileDownloader;

var downloader = new FileDownloader();

const string downloadUrl = @"https://stewartcelani-public.s3.amazonaws.com/samplefiles/1.tmp";
string downloadPath = Path.Combine(Path.GetTempPath(), "1.tmp");

/*
 * Example 1: DownloadAsync basic usage
 */
FileInfo downloadAsyncExample1 = await downloader.DownloadAsync(downloadUrl, downloadPath);
Console.WriteLine(downloadAsyncExample1.FullName);
Console.WriteLine();

/*
 * Example 2: DownloadAsync overload
 */
var example2File = new FileDownloaderFile()
{
    DownloadUrl = downloadUrl,
    DownloadPath = Path.Combine(Path.GetTempPath(), "2.tmp")
};
FileInfo downloadAsyncExample2 = await downloader.DownloadAsync(example2File);
Console.WriteLine(downloadAsyncExample2.FullName);
Console.WriteLine();

/*
 * Example 3: DownloadParallelAsync
 */
List<FileDownloaderFile> fileList = new();
for (var i = 1; i <= 10; i++)
{
    fileList.Add(new FileDownloaderFile()
    {
        DownloadUrl = downloadUrl,
        DownloadPath = Path.Combine(Path.GetTempPath(), $"{i + 2}.tmp")
    });
}
List<FileInfo> downloadParallelAsyncExample = await downloader.DownloadParallelAsync(fileList);
foreach (FileInfo fileInfo in downloadParallelAsyncExample)
{
    Console.WriteLine(fileInfo.FullName);
}
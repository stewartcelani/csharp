namespace Library.FileDownloader;

public class FileDownloader
{
    private readonly HttpClient _http = new();

    public async Task DownloadAsync(string downloadUrl, string downloadPath, CancellationToken ct)
    {
        await using Stream stream = await _http.GetStreamAsync(downloadUrl, ct);
        await using FileStream fileStream = new (downloadPath, FileMode.Create);
        await stream.CopyToAsync(fileStream, ct);
    }

    public async Task DownloadParallelAsync(
        IEnumerable<FileDownloaderFile> fileList, CancellationToken ct, int maxDegreeOfParallelism = 10)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism,
            CancellationToken = ct
        };
        
        await Parallel.ForEachAsync(fileList, parallelOptions, async (file, cti) =>
        {
            await DownloadAsync(file.DownloadUrl, file.DownloadPath, cti);
        });
    }
    
}
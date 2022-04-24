namespace Library.FileDownloader;

public class FileDownloader
{
    private readonly HttpClient _http = new();

    public async Task<FileInfo> DownloadAsync(
        string downloadUrl, string downloadPath, CancellationToken ct = new CancellationToken())
    {
        await using Stream stream = await _http.GetStreamAsync(downloadUrl, ct);
        await using FileStream fileStream = new (downloadPath, FileMode.Create);
        await stream.CopyToAsync(fileStream, ct);
        return new FileInfo(downloadPath);
    }

    public Task<FileInfo> DownloadAsync(FileDownloaderFile file, CancellationToken ct = new CancellationToken())
    {
        return DownloadAsync(file.DownloadUrl, file.DownloadPath, ct);
    }

    public async Task<List<FileInfo>> DownloadParallelAsync(
        IEnumerable<FileDownloaderFile> fileList, CancellationToken ct = new CancellationToken(), int maxDegreeOfParallelism = 10)
    {
        List<FileInfo> fileInfoList = new();
        
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism,
            CancellationToken = ct
        };
        
        await Parallel.ForEachAsync(fileList, parallelOptions, async (file, cti) =>
        {
            FileInfo fileInfo = await DownloadAsync(file.DownloadUrl, file.DownloadPath, cti);
            fileInfoList.Add(fileInfo);
        });

        return fileInfoList;
    }
    
}
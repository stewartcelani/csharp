namespace FileSeeder;

public class FileSeeder
{
    private readonly FileSeederConfiguration _config;
    private readonly Random _random = new();
    private DirectoryInfo _rootDirectory = null!;

    public FileSeeder()
    {
        var config = new FileSeederConfiguration();
        ValidateFileSeederConfiguration(config);
        _config = config;
    }

    public FileSeeder(FileSeederConfiguration config)
    {
        ValidateFileSeederConfiguration(config);
        _config = config;
    }

    public List<FileInfo> Files { get; } = new();

    private void ValidateFileSeederConfiguration(FileSeederConfiguration config)
    {
        // Validate path
        if (!Directory.Exists(config.Path)) Directory.CreateDirectory(config.Path);
        _rootDirectory = new DirectoryInfo(config.Path);

        // Check for permissions on path
        var tempFilePath = Path.Combine(config.Path, $"{Guid.NewGuid()}.tmp");
        File.Create(tempFilePath).Close();
        File.Delete(tempFilePath);

        // Check for valid file extensions
        if (config.FileExtensions.Length == 0)
            throw new ArgumentException("FileExtensions can not be empty.");
        if (config.FileExtensions.Any(x => x.Contains('.')))
            throw new ArgumentException("FileExtensions can not contain '.'.");
        if (config.FileExtensions.Any(x => x.Length > 4))
            throw new ArgumentException("FileExtensions can not be longer than 4 characters.");
    }


    public List<FileInfo> SeedFiles(int numberOfFilesToSeed, int? fileSizeInBytes = null)
    {
        for (var i = 1; i <= numberOfFilesToSeed; i++)
        {
            var fileName = $"{i}.{GetRandomFileExtension()}";
            var iFileSizeInBytes = fileSizeInBytes ?? GetRandomFileSizeInBytes();
            var file = SeedFile(fileName, iFileSizeInBytes);
            Files.Add(file);
        }

        return Files;
    }

    public FileInfo SeedFile(string fileName, int fileSizeInBytes = 0)
    {
        var path = Path.Combine(_rootDirectory.ToString(), fileName);
        File.WriteAllBytes(path, new byte[fileSizeInBytes]);
        return new FileInfo(path);
    }

    public string GetRandomFileExtension()
    {
        return _config.FileExtensions[_random.Next(0, _config.FileExtensions.Length)];
    }

    private int GetRandomFileSizeInBytes()
    {
        var maxFileSizeInBytes = _config.MaxFileSizeInMb * 1000000;
        return _random.Next(0, maxFileSizeInBytes + 1);
    }
}
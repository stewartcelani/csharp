namespace Library.FileSeeder;

public class FileSeeder
{
    private readonly Random _random = new Random();
    private readonly FileSeederConfiguration _config;
    private DirectoryInfo _rootDirectory = null!;

    public List<FileInfo> Files { get; } = new();

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

    private void ValidateFileSeederConfiguration(FileSeederConfiguration config)
    {
        // Validate path
        if (!Directory.Exists(config.Path))
        {
            Directory.CreateDirectory(config.Path);
        }
        _rootDirectory = new DirectoryInfo(config.Path);
        
        // Check for permissions on path
        string tempFilePath = Path.Combine(config.Path, $"{Guid.NewGuid()}.tmp");
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

    public List<FileInfo> SeedFiles(int numberOfFilesToSeed = 10)
    {
        return SeedFiles(numberOfFilesToSeed, GetRandomFileSizeInBytes());
    }
    
    public List<FileInfo> SeedFiles(int numberOfFilesToSeed = 10, int fileSizeInBytes = 0)
    {
        for (var i = 1; i <= numberOfFilesToSeed; i++)
        {
            string fileName = $"{i}.{GetRandomFileExtension()}";
            FileInfo file = SeedFile(fileName, fileSizeInBytes);
            Files.Add(file);
        }
        return Files;
    }

    public FileInfo SeedFile(string fileName, int fileSizeInBytes = 0)
    {
        string path = Path.Combine(_rootDirectory.ToString(), fileName);
        File.WriteAllBytes(path, new byte[fileSizeInBytes]);
        return new FileInfo(path);
    }

    public string GetRandomFileExtension()
    {
        return _config.FileExtensions[_random.Next(0, _config.FileExtensions.Length)];
    }

    private int GetRandomFileSizeInBytes()
    {
        int maxFileSizeInBytes = _config.MaxFileSizeInMb * 1000000;
        return _random.Next(0, maxFileSizeInBytes + 1);
    }
    
}
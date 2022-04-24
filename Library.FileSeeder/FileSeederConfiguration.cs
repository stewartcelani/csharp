namespace Library.FileSeeder;

public class FileSeederConfiguration
{
    public string Path { get; set; } = Directory.GetCurrentDirectory();
    public int MaxFileSizeInMb { get; set; } = 333;
    public string[] FileExtensions { get; set; } = new[] { "pdf", "docx", "xlsx", "txt", "dwg", "psd", "avi" };
}
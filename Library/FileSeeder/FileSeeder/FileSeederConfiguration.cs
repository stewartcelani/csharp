namespace FileSeeder;

public class FileSeederConfiguration
{
    public string Path { get; set; } = Directory.GetCurrentDirectory();
    public int MaxFileSizeInMb { get; set; } = 1;
    public string[] FileExtensions { get; set; } = { "pdf", "docx", "xlsx", "txt", "dwg", "psd", "avi" };
}
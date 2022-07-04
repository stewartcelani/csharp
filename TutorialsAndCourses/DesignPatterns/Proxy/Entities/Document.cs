namespace Proxy.Entities;

public class Document : IDocument
{
    private string _fileName;
    
    public string? Title { get; private set; }
    public string? Content { get; private set; }
    public int AuthorId { get; private set; }
    public DateTimeOffset LastAccessed { get; private set; }

    public Document(string fileName)
    {
        _fileName = fileName;
        LoadDocument(fileName);
    }

    private void LoadDocument(string fileName)
    {
        Console.WriteLine("Executing expensive action: loading a file from disk.");
        // fake loading...
        Thread.Sleep(2500);

        Title = "An expensive document";
        Content = "Lots and lots of content";
        AuthorId = 1;
        LastAccessed = DateTimeOffset.Now;
    }

    public void DisplayDocument()
    {
        Console.WriteLine($"Title: {Title}, Content: {Content}");
    }
}
using Proxy.Entities;

namespace Proxy.VirtualProxy;

public class DocumentProxy : IDocument
{
    //private Document? _document;
    private Lazy<Document> _document;
    private string _fileName;

    public DocumentProxy(string fileName)
    {
        _fileName = fileName;
        _document = new Lazy<Document>(() => new Document(_fileName));
    }

    public void DisplayDocument()
    {
        // (_document ??= new Document(_fileName)).DisplayDocument();
        _document.Value.DisplayDocument();
    }
}
using Proxy.Entities;
using Proxy.VirtualProxy;

namespace Proxy.ProtectionProxy;

public class ProtectedDocumentProxy : IDocument
{
    private string _fileName;
    private string _userRole;
    private DocumentProxy _documentProxy;

    public ProtectedDocumentProxy(string fileName, string userRole)
    {
        _fileName = fileName;
        _userRole = userRole;
        _documentProxy = new DocumentProxy(_fileName);
    }


    public void DisplayDocument()
    {
        Console.WriteLine($"Entering DisplayDocument in {nameof(ProtectedDocumentProxy)}.");

        if (_userRole != "Viewer")
        {
            throw new UnauthorizedAccessException();
        }
        
        _documentProxy.DisplayDocument();
        
        Console.WriteLine($"Exiting DisplayDocument in {nameof(ProtectedDocumentProxy)}.");
    }
}
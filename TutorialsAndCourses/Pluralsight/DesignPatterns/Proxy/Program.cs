using Proxy.Entities;
using Proxy.ProtectionProxy;
using Proxy.VirtualProxy;

// without proxy the expensive construction of document (loading from disk) happens upfront instead of on first use
Console.WriteLine("Constructing document.");
var myDocument = new Document("MyDocument.pdf");
Console.WriteLine("Document constructed.");
myDocument.DisplayDocument();
Console.WriteLine();

// with proxy the expensive action is executed only when we need it to (Lazy-initialization)
Console.WriteLine("Constructing document proxy.");
var myDocumentProxy = new DocumentProxy("MyDocument.pdf");
Console.WriteLine("Document proxy constructed.");
myDocumentProxy.DisplayDocument();
Console.WriteLine();

// with chained proxy with correct role
Console.WriteLine("Constructing protected document proxy.");
var myProtectedDocumentProxy = new ProtectedDocumentProxy("MyDocument.pdf", "Viewer");
Console.WriteLine("Protected document proxy constructed.");
myProtectedDocumentProxy.DisplayDocument();
Console.WriteLine();

// with chained proxy, no access, will throw UnauthorizedAccessException
Console.WriteLine("Constructing protected document proxy.");
var myProtectedDocumentProxy2 = new ProtectedDocumentProxy("MyDocument.pdf", "NotAViewer");
Console.WriteLine("Protected document proxy constructed.");
myProtectedDocumentProxy2.DisplayDocument();

Console.ReadKey();
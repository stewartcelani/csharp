using System.ComponentModel.DataAnnotations;

namespace ChainOfResponsibility;

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentLastModifiedHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;
    
    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }

    public void Handle(Document document)
    {
        if (document.LastModified < DateTime.UtcNow.AddDays(-30))
        {
            throw new ValidationException(
                new ValidationResult(
                    "Document must be modified in the last 30 days",
                    new List<string>() { "LastModified" }),
                null,
                null);
        }
        
        // go to the next handler if there is one set
        _successor?.Handle(document);
    }
}
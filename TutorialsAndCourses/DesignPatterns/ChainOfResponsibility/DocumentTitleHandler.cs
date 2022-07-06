using System.ComponentModel.DataAnnotations;

namespace ChainOfResponsibility;

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentTitleHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;
    
    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }

    public void Handle(Document document)
    {
        if (document.Title == string.Empty)
        {
            throw new ValidationException(
                new ValidationResult(
                    "Title must be filled out",
                    new List<string>() { "Title" }),
                null,
                null);
        }
        
        // go to the next handler if there is one set
        _successor?.Handle(document);
    }
}
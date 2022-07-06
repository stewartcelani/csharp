using System.ComponentModel.DataAnnotations;

namespace ChainOfResponsibility;

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentApprovedByLitigationHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;
    
    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }

    public void Handle(Document document)
    {
        if (!document.ApprovedByLitigation)
        {
            throw new ValidationException(
                new ValidationResult(
                    "Document must be approved by litigation",
                    new List<string>() { "ApprovedByLitigation" }),
                null,
                null);
        }
        
        // go to the next handler if there is one set
        _successor?.Handle(document);
    }
}
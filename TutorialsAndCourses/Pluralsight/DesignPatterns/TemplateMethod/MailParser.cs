namespace TemplateMethod;

/// <summary>
/// Abstract Class
/// </summary>
public abstract class MailParser
{
    protected virtual void FindServer()
    {
        Console.WriteLine("Finding server...");
    }

    protected abstract void AuthenticateToServer();

    private string ParseHtmlMailBody(string identifier)
    {
        Console.WriteLine("Parsing HTML mail body...");
        return $"This is the body of the mail with id {identifier}";
    }

    /// <summary>
    /// Template method
    /// </summary>
    public string ParseMailBody(string identifier)
    {
        Console.WriteLine("Parsing mail body (in template method)...");
        FindServer();
        AuthenticateToServer();
        return ParseHtmlMailBody(identifier);
    }
}
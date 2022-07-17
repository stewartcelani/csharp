namespace TemplateMethod;

public class ExchangeMailParser : MailParser
{
    protected override void AuthenticateToServer()
    {
        Console.WriteLine("Connecting to Exchange");
    }
}
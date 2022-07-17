namespace TemplateMethod;

public class ApacheMailParser : MailParser
{
    protected override void AuthenticateToServer()
    {
        Console.WriteLine("Connecting to Apache");
    }
}
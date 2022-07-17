namespace TemplateMethod;

public class EudoraMailParser : MailParser
{
    protected override void FindServer()
    {
        Console.WriteLine("Finding Eudora server through a custom algorithm...");
    }

    protected override void AuthenticateToServer()
    {
        Console.WriteLine("Connecting to Eudora");
    }
}
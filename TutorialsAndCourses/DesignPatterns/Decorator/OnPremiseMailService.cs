namespace Decorator;

public class OnPremiseMailService : IMailService
{
    public bool SendMail(string message)
    {
        Console.WriteLine($"Message {message} sent via {nameof(OnPremiseMailService)}");
        return true;
    }
}
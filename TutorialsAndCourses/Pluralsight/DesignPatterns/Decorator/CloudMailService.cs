namespace Decorator;

public class CloudMailService : IMailService
{
    public bool SendMail(string message)
    {
        Console.WriteLine($"Message {message} sent via {nameof(CloudMailService)}");
        return true;
    }
}
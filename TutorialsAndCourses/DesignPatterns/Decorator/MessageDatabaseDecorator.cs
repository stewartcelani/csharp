namespace Decorator;

public class MessageDatabaseDecorator : MailServiceDecoratorBase
{
    public List<string> SentMessages { get; } = new();
    
    public MessageDatabaseDecorator(IMailService mailService) : base(mailService)
    {
    }

    public override bool SendMail(string message)
    {
        if (base.SendMail(message))
        {
            SentMessages.Add(message);
            return true;
        }

        return false;
    }
}
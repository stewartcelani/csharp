namespace Decorator;

public class StatisticsDecorator : MailServiceDecoratorBase
{
    public StatisticsDecorator(IMailService mailService) : base(mailService)
    {
    }

    public override bool SendMail(string message)
    {
        // Fake collecting statistics
        Console.WriteLine($"Collecting statistics in {nameof(StatisticsDecorator)}.");
        return base.SendMail(message);
    }
}
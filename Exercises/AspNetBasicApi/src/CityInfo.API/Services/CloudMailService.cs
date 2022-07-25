using CityInfo.API.Domain.Settings;
using CityInfo.API.Logging;

namespace CityInfo.API.Services;

public class CloudMailService : IMailService
{
    private ILoggerAdapter<CloudMailService> _logger;

    private readonly string _mailFrom;
    private readonly string _mailTo;
    

    public CloudMailService(ILoggerAdapter<CloudMailService> logger, MailSettings mailSettings)
    {
        _logger = logger;
        _mailTo = mailSettings.MailToAddress;
        _mailFrom = mailSettings.MailFromAddress;
    }

    public Task Send(string subject, string message)
    {
        _logger.LogInformation("Mail from {mailFrom} to {mailTo} with {className}", _mailFrom, _mailTo, nameof(CloudMailService));
        _logger.LogInformation("Subject: {subject}", subject);
        _logger.LogInformation("Message: {message}", message);
        return Task.CompletedTask;
    }
}
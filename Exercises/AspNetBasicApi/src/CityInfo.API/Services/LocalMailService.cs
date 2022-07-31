using System.Diagnostics.CodeAnalysis;
using CityInfo.API.Domain.Settings;
using CityInfo.API.Logging;

namespace CityInfo.API.Services;

[ExcludeFromCodeCoverage]
public class LocalMailService : IMailService
{
    private readonly string _mailFrom;
    private readonly string _mailTo;
    private readonly ILoggerAdapter<LocalMailService> _logger;

    public LocalMailService(ILoggerAdapter<LocalMailService> logger, MailSettings mailSettings)
    {
        _logger = logger;
        _mailTo = mailSettings.MailToAddress;
        _mailFrom = mailSettings.MailFromAddress;
    }

    public Task Send(string subject, string message)
    {
        _logger.LogInformation("Mail from {mailFrom} to {mailTo} with {className}", _mailFrom, _mailTo,
            nameof(LocalMailService));
        _logger.LogInformation("Subject: {subject}", subject);
        _logger.LogInformation("Message: {message}", message);
        return Task.CompletedTask;
    }
}
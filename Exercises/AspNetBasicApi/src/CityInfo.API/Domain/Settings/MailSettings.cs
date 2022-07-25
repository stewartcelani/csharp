namespace CityInfo.API.Domain.Settings;

public class MailSettings
{
    public string MailFromAddress { get; init; } = default!;
    public string MailToAddress { get; init; } = default!;
}
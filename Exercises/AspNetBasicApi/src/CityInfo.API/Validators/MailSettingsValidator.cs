using CityInfo.API.Domain.Settings;
using FluentValidation;

namespace CityInfo.API.Validators;

public class MailSettingsValidator : AbstractValidator<MailSettings>
{
    public MailSettingsValidator()
    {
        RuleFor(x => x.MailToAddress).NotEmpty().SetValidator(new EmailValidator());
        RuleFor(x => x.MailFromAddress).NotEmpty().SetValidator(new EmailValidator());
    }
}
using System.Text.RegularExpressions;
using FluentValidation;

namespace CityInfo.API.Validators;

public class EmailValidator : AbstractValidator<string>
{
    private static readonly Regex EmailRegex =
        new("^[\\w!#$%&’*+/=?`{|}~^-]+(?:\\.[\\w!#$%&’*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public EmailValidator()
    {
        RuleFor(x => x).NotEmpty().Matches(EmailRegex);
    }
}
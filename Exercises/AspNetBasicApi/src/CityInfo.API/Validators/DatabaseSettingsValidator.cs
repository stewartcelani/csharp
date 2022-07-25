using CityInfo.API.Domain.Settings;
using FluentValidation;

namespace CityInfo.API.Validators;

public class DatabaseSettingsValidator : AbstractValidator<DatabaseSettings>
{
    public DatabaseSettingsValidator()
    {
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.EnableSensitiveDataLogging).NotNull();
    }
}
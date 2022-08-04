using CityInfo.API.Domain.Settings;
using FluentValidation;

namespace CityInfo.API.Validators;

public class CacheSettingsValidator : AbstractValidator<CacheSettings>
{
    public CacheSettingsValidator()
    {
        RuleFor(x => x.Enabled).NotNull();
        RuleFor(x => x.ConnectionString).NotEmpty().MinimumLength(5);
    }
    
}
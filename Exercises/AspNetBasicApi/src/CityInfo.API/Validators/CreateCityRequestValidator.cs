using CityInfo.API.Contracts.Requests;
using FluentValidation;

namespace CityInfo.API.Validators;

public class CreateCityRequestValidator : AbstractValidator<CreateCityRequest>
{
    public CreateCityRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
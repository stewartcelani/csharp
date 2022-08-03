using CityInfo.API.Contracts.v1.Requests;
using FluentValidation;

namespace CityInfo.API.Validators;

public class CreatePointOfInterestRequestValidator : AbstractValidator<CreatePointOfInterestRequest>
{
    public CreatePointOfInterestRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
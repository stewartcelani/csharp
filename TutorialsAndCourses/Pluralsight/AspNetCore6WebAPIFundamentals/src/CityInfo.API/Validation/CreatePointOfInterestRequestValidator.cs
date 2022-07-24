using CityInfo.API.Contracts.Requests;
using FluentValidation;

namespace CityInfo.API.Validation;

public class CreatePointOfInterestRequestValidator : AbstractValidator<CreatePointOfInterestRequest>
{
    public CreatePointOfInterestRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
    }
}
using CityInfo.API.Contracts.Requests;
using FluentValidation;

namespace CityInfo.API.Validators;

public class UpdatePointOfInterestRequestValidator : AbstractValidator<UpdatePointOfInterestRequest>
{
    public UpdatePointOfInterestRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CreatePointOfInterestRequest).NotEmpty()
            .SetValidator(new CreatePointOfInterestRequestValidator());
    }
}
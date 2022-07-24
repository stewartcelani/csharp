using CityInfo.API.Contracts.Requests;
using FluentValidation;

namespace CityInfo.API.Validation;

public class UpdatePointOfInterestRequestValidator : AbstractValidator<UpdatePointOfInterestRequest>
{
    public UpdatePointOfInterestRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CreatePointOfInterestRequest).NotNull().SetValidator(new CreatePointOfInterestRequestValidator());
    }
}
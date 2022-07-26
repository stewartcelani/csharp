using FluentValidation.Results;

namespace CityInfo.API.Validators.Helpers;

public static class ValidationFailureHelper
{
    public static IEnumerable<ValidationFailure> Generate(string paramName, string message)
    {
        return new[]
        {
            new ValidationFailure(paramName, message)
        };
    }
}
using FluentValidation.Results;

namespace CityInfo.API.Validation;

public static class ValidationHelper
{
    public static IEnumerable<ValidationFailure> GenerateValidationError(string paramName, string message)
    {
        return new []
        {
            new ValidationFailure(paramName, message)
        };
    }
}
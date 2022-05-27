using FluentValidation.Results;

namespace AppFramework.Common.Validations
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T model);
    }
}
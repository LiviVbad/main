using FluentValidation.Results;

namespace AppFramework.Common.Core
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T model);
    }
}
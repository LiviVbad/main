using FluentValidation.Results;

namespace AppFramework.Shared.Common.Core
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T model);
    }
}
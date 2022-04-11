using FluentValidation.Results;

namespace AppFrameworkDemo.Shared
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T model);
    }
}
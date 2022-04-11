using AppFrameworkDemo.Shared.Models;
using FluentValidation;

namespace AppFrameworkDemo.Shared.Validations
{
    public class CreateEditionValidator : AbstractValidator<EditionCreateModel>
    {
        public CreateEditionValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired();
        }
    }
}
using AppFramework.Models;
using AppFramework.Shared.Validations;
using FluentValidation;

namespace AppFramework.Validations
{
    public class CreateEditionValidator : AbstractValidator<EditionCreateModel>
    {
        public CreateEditionValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired();
        }
    }
}
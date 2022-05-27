using AppFramework.Common.Models;
using FluentValidation;

namespace AppFramework.Common.Validations
{
    public class CreateEditionValidator : AbstractValidator<EditionCreateModel>
    {
        public CreateEditionValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired();
        }
    }
}
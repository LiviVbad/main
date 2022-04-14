using AppFramework.Shared.Common.Models;
using FluentValidation;

namespace AppFramework.Shared.Common.Core.Validations
{
    public class CreateEditionValidator : AbstractValidator<EditionCreateModel>
    {
        public CreateEditionValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired();
        }
    }
}
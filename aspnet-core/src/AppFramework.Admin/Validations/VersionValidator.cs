using AppFramework.Models;
using AppFramework.Shared.Validations;
using FluentValidation; 

namespace AppFramework.Validations
{
    public class VersionValidator : AbstractValidator<VersionListModel>
    {
        public VersionValidator()
        {
            RuleFor(x => x.Name).IsRequired();
            RuleFor(x => x.Version).IsRequired();
        }
    }
}

using Abp.Organizations;
using AppFrameworkDemo.Shared.Models;
using FluentValidation;

namespace AppFrameworkDemo.Shared.Validations
{
    public class OrganizationUnitValidator : AbstractValidator<CreateOrganizationUnitModel>
    {
        public OrganizationUnitValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired().MaxLength(OrganizationUnit.MaxDisplayNameLength);
        }
    }
}
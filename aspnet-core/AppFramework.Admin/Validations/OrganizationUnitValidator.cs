using Abp.Organizations;
using AppFramework.Models;
using AppFramework.Shared.Validations;
using FluentValidation;

namespace AppFramework.Validations
{
    public class OrganizationUnitValidator : AbstractValidator<CreateOrganizationUnitModel>
    {
        public OrganizationUnitValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired().MaxLength(OrganizationUnit.MaxDisplayNameLength);
        }
    }
}
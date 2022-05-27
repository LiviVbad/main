using Abp.Organizations;
using AppFramework.Common.Models; 
using FluentValidation;

namespace AppFramework.Common.Validations
{
    public class OrganizationUnitValidator : AbstractValidator<CreateOrganizationUnitModel>
    {
        public OrganizationUnitValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired().MaxLength(OrganizationUnit.MaxDisplayNameLength);
        }
    }
}
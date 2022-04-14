using Abp.Organizations;
using AppFramework.Shared.Common.Models; 
using FluentValidation;

namespace AppFramework.Shared.Common.Core.Validations
{
    public class OrganizationUnitValidator : AbstractValidator<CreateOrganizationUnitModel>
    {
        public OrganizationUnitValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired().MaxLength(OrganizationUnit.MaxDisplayNameLength);
        }
    }
}
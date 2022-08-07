using AppFramework.Common.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Common.Validations
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

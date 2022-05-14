using AppFramework.Common;
using Prism.Ioc;
using System;
using System.Windows.Markup;

namespace AppFramework.Extensions
{
    public class HasPermissionExtension : MarkupExtension
    {
        public string Text { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return false;

            var permissionService = ContainerLocator.Container.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}
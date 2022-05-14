using Abp.AspNetCore.Mvc.ViewComponents;

namespace AppFramework.Web.Public.Views
{
    public abstract class AppFrameworkDemoViewComponent : AbpViewComponent
    {
        protected AppFrameworkDemoViewComponent()
        {
            LocalizationSourceName = AppFrameworkConsts.LocalizationSourceName;
        }
    }
}
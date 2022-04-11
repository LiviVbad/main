using Abp.AspNetCore.Mvc.ViewComponents;

namespace AppFrameworkDemo.Web.Public.Views
{
    public abstract class AppFrameworkDemoViewComponent : AbpViewComponent
    {
        protected AppFrameworkDemoViewComponent()
        {
            LocalizationSourceName = AppFrameworkDemoConsts.LocalizationSourceName;
        }
    }
}
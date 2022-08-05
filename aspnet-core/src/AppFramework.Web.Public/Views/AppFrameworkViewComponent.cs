using Abp.AspNetCore.Mvc.ViewComponents;

namespace AppFramework.Web.Public.Views
{
    public abstract class AppFrameworkViewComponent : AbpViewComponent
    {
        protected AppFrameworkViewComponent()
        {
            LocalizationSourceName = AppFrameworkConsts.LocalizationSourceName;
        }
    }
}
using Abp.AspNetCore.Mvc.Views;

namespace AppFramework.Web.Views
{
    public abstract class AppFrameworkDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected AppFrameworkDemoRazorPage()
        {
            LocalizationSourceName = AppFrameworkConsts.LocalizationSourceName;
        }
    }
}

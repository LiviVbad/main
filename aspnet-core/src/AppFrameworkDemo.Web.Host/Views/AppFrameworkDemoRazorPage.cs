using Abp.AspNetCore.Mvc.Views;

namespace AppFrameworkDemo.Web.Views
{
    public abstract class AppFrameworkDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected AppFrameworkDemoRazorPage()
        {
            LocalizationSourceName = AppFrameworkDemoConsts.LocalizationSourceName;
        }
    }
}

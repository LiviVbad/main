using Abp.AspNetCore.Mvc.Views;

namespace AppFramework.Web.Views
{
    public abstract class AppFrameworkRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected AppFrameworkRazorPage()
        {
            LocalizationSourceName = AppFrameworkConsts.LocalizationSourceName;
        }
    }
}

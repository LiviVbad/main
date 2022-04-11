using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace AppFrameworkDemo.Web.Public.Views
{
    public abstract class AppFrameworkDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected AppFrameworkDemoRazorPage()
        {
            LocalizationSourceName = AppFrameworkDemoConsts.LocalizationSourceName;
        }
    }
}

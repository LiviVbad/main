using Abp.Domain.Services;

namespace AppFrameworkDemo
{
    public abstract class AppFrameworkDemoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected AppFrameworkDemoDomainServiceBase()
        {
            LocalizationSourceName = AppFrameworkDemoConsts.LocalizationSourceName;
        }
    }
}

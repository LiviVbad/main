using Abp.Domain.Services;

namespace AppFramework
{
    public abstract class AppFrameworkDemoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected AppFrameworkDemoDomainServiceBase()
        {
            LocalizationSourceName = AppFrameworkConsts.LocalizationSourceName;
        }
    }
}

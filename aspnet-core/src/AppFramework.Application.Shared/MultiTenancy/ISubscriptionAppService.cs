using Abp.Application.Services;
using System.Threading.Tasks;

namespace AppFramework.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
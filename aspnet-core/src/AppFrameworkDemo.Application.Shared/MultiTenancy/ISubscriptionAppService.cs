using Abp.Application.Services;
using System.Threading.Tasks;

namespace AppFrameworkDemo.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
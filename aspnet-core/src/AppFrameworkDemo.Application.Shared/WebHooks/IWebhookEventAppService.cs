using Abp.Webhooks;
using System.Threading.Tasks;

namespace AppFrameworkDemo.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
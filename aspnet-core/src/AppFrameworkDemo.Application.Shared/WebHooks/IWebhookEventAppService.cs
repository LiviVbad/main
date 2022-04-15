using Abp.Webhooks;
using System.Threading.Tasks;

namespace AppFramework.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
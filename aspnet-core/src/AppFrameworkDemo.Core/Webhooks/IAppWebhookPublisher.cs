using System.Threading.Tasks;
using AppFrameworkDemo.Authorization.Users;

namespace AppFrameworkDemo.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}

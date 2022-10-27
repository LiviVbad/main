using System;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Signalr
{
    public interface ISignalrService
    {
        bool IsConnected { get; }

        Task StartAsync();

        Task StopAsync();

        Task SendMessage(SendChatMessageInput input);
    }

    public class SendChatMessageInput
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public string Message { get; set; }
    }
}

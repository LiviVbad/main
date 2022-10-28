using System;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Signalr
{
    public interface IFriendChatService
    {
        bool IsConnected { get; }

        Task StartAsync();

        Task StopAsync();

        Task SendMessage(SendChatMessageInput input);

        Task GetUserChatFriendsAsync();
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

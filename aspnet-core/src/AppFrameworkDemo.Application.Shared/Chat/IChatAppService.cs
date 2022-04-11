using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.Chat.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Chat
{
    public interface IChatAppService : IApplicationService
    {
        GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings();

        Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input);

        Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input);
    }
}
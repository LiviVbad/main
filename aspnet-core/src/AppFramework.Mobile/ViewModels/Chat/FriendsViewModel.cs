using AppFramework.ApiClient;
using AppFramework.Shared.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class FriendsViewModel : RegionViewModel
    {
        private readonly IApplicationContext context;
        public IFriendChatService chatService { get; private set; }

        public FriendsViewModel(IApplicationContext context,
            IFriendChatService chatService)
        {
            this.context = context;
            this.chatService = chatService;
        }

        public override async Task RefreshAsync()
        {
            await chatService.GetUserChatFriendsAsync(); 
        } 
    }
}

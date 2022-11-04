using AppFramework.ApiClient;
using AppFramework.Shared.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.ViewModels
{
    public class FriendsViewModel : NavigationViewModel
    {
        private readonly IApplicationContext context;
        public IFriendChatService chatService { get; private set; }

        public FriendsViewModel(IApplicationContext context,
            IFriendChatService chatService)
        {
            this.context = context;
            this.chatService = chatService;
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await chatService.GetUserChatFriendsAsync();
            await chatService.StartAsync();
        }
    }
}

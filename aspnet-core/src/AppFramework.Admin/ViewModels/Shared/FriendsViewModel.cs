using AppFramework.ApiClient;
using AppFramework.Friendships;
using AppFramework.Shared;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services.Signalr;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class FriendsViewModel : NavigationViewModel
    {
        private readonly IFriendshipAppService friendshipAppService;
        public IFriendChatService chatService { get; private set; }
        private readonly IApplicationContext context;

        public DelegateCommand AddUserCommand { get; private set; }
        public DelegateCommand<FriendModel> ClickChatCommand { get; private set; }

        public FriendsViewModel(IApplicationContext context,
            IFriendChatService chatService)
        {
            this.chatService=chatService;
            this.context=context;
            AddUserCommand=new DelegateCommand(AddUser);
            ClickChatCommand=new DelegateCommand<FriendModel>(ClickChat);
        }

        private async void ClickChat(FriendModel obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", obj);
            await dialog.ShowDialogAsync(AppViews.FriendsChat, param);
            obj.UnreadMessageCount=0;
        }

        private async void AddUser()
        {
            var result = await dialog.ShowDialogAsync(AppViews.SelectedUser);
            if (result.Result== Prism.Services.Dialogs.ButtonResult.OK)
            {
                var userId = result.Parameters.GetValue<int>("Value");

                await WebRequest.Execute(() => friendshipAppService.CreateFriendshipRequest(
                    new Friendships.Dto.CreateFriendshipRequestInput()
                    {
                        UserId=userId,
                        TenantId=context.CurrentTenant?.TenantId
                    }), async result =>
                    {
                        await chatService.GetUserChatFriendsAsync();
                    })
                ;
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await chatService.GetUserChatFriendsAsync();
            await chatService.StartAsync();
        }
    }
}

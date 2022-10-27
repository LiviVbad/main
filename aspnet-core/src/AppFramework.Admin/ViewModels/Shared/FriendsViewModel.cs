using AppFramework.ApiClient;
using AppFramework.Chat;
using AppFramework.Friendships;
using AppFramework.Shared;
using Models.Chat;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class FriendsViewModel : NavigationViewModel
    {
        private readonly IChatAppService chatAppService;
        private readonly IFriendshipAppService friendshipAppService;
        private readonly IApplicationContext context;
        private ObservableCollection<FriendModel> friends;

        public ObservableCollection<FriendModel> Friends
        {
            get { return friends; }
            set { friends = value; RaisePropertyChanged(); }
        }

        public DelegateCommand AddUserCommand { get; private set; }
        public DelegateCommand<FriendModel> ClickChatCommand { get; private set; }

        public FriendsViewModel(IChatAppService chatAppService,
            IFriendshipAppService friendshipAppService,
            IApplicationContext context)
        {
            friends = new ObservableCollection<FriendModel>();
            this.chatAppService = chatAppService;
            this.friendshipAppService=friendshipAppService;
            this.context=context;
            AddUserCommand=new DelegateCommand(AddUser);
            ClickChatCommand=new DelegateCommand<FriendModel>(ClickChat);
        }

        private async void ClickChat(FriendModel obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", obj);
            await dialog.ShowDialogAsync(AppViews.FriendsChat, param);
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
                        await GetUserChatFriendsWithSettings();
                    })
                ;
            }
        }


        private async Task GetUserChatFriendsWithSettings()
        {
            await WebRequest.Execute(async () =>
            {
                var frientsSetting = await chatAppService.GetUserChatFriendsWithSettings();
                if (frientsSetting!=null&&frientsSetting.Friends != null)
                {
                    Friends.Clear();
                    var friendsList = Map<List<FriendModel>>(frientsSetting.Friends);

                    foreach (var item in friendsList)
                    {
                        if (string.IsNullOrWhiteSpace(item.FriendTenancyName))
                        {
                            item.FriendTenancyName="Host";
                        }

                        Friends.Add(item);
                    }
                }
            });
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await GetUserChatFriendsWithSettings();
        }
    }
}

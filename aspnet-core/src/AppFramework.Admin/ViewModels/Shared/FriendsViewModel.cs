using AppFramework.Chat;
using AppFramework.Shared;
using Models.Chat;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class FriendsViewModel : NavigationViewModel
    {
        private readonly IChatAppService chatAppService;

        private ObservableCollection<FriendModel> friends;

        public ObservableCollection<FriendModel> Friends
        {
            get { return friends; }
            set { friends = value; RaisePropertyChanged(); }
        }

        public FriendsViewModel(IChatAppService chatAppService)
        {
            friends = new ObservableCollection<FriendModel>();
            this.chatAppService = chatAppService;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await WebRequest.Execute(async () =>
             {
                 var frientsSetting = await chatAppService.GetUserChatFriendsWithSettings();
                 if (frientsSetting.Friends != null)
                 {
                     Friends.Clear();
                     var friendsList = Map<List<FriendModel>>(frientsSetting.Friends);

                     foreach (var item in friendsList)
                         Friends.Add(item);
                 }
             });
        }
    }
}

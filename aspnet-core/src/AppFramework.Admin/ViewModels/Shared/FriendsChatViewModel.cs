using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services.Signalr;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        private readonly IChatAppService chatApp;
        private readonly IFriendChatService chatService;

        private FriendModel friend;

        public FriendModel Friend
        {
            get { return friend; }
            set { friend = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ChatMessageModel> messages;

        public ObservableCollection<ChatMessageModel> Messages
        {
            get { return messages; }
            set { messages = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SendCommand { get; private set; }

        public FriendsChatViewModel(
            IChatAppService chatApp,
            IFriendChatService chatService)
        {
            this.chatApp=chatApp;
            this.chatService=chatService;
            messages=new ObservableCollection<ChatMessageModel>();
            SendCommand =new DelegateCommand(Send);
        }

        private void Send()
        {
            chatService.SendMessage(new SendChatMessageInput()
            {
                UserId= Friend.FriendUserId,
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Friend= parameters.GetValue<FriendModel>("Value");

                await GetUserChatMessagesByUser(Friend.FriendUserId);
            }
        }

        private async Task GetUserChatMessagesByUser(long userId)
        {
            await WebRequest.Execute(() =>
                chatApp.GetUserChatMessages(new Chat.Dto.GetUserChatMessagesInput()
                {
                    UserId=userId
                }), async result =>
                {
                    var list = Map<List<ChatMessageModel>>(result.Items);
                    foreach (var item in list)
                    {
                        var friendUser = chatService.Friends
                        .FirstOrDefault(t => t.FriendUserId.Equals(item.TargetUserId));

                        if (friendUser!=null)
                        {
                            item.UserName=friendUser.FriendUserName;
                            Messages.Add(item);
                        } 
                    }
                    await Task.CompletedTask;
                });
        }
    }
}

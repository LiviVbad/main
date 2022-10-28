using AppFramework.ApiClient;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services.Signalr;
using Prism.Commands;
using Prism.Services.Dialogs;
using Syncfusion.Windows.Tools.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace AppFramework.ViewModels
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        private readonly IApplicationContext context;
        private readonly IChatAppService chatApp;
        private readonly IFriendChatService chatService;

        private string userName;

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

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SendCommand { get; private set; }

        public FriendsChatViewModel(IApplicationContext context,
            IChatAppService chatApp,
            IFriendChatService chatService)
        {
            this.context=context;
            this.chatApp=chatApp;
            this.chatService=chatService;
            chatService.OnChatMessageHandler+=ChatService_OnChatMessageHandler;
            messages =new ObservableCollection<ChatMessageModel>();
            SendCommand =new DelegateCommand(Send);
        }

        private void ChatService_OnChatMessageHandler(ChatMessageDto chatMessage)
        {
            var msg = Map<ChatMessageModel>(chatMessage);
            UpdateMessageInfo(msg);
            Messages.Add(msg);
        }

        protected override void Cancel()
        {
            chatService.OnChatMessageHandler-=ChatService_OnChatMessageHandler;
            base.Cancel();
        }

        private async void Send()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;

            await chatService.SendMessage(new SendChatMessageInput()
            {
                UserId= Friend.FriendUserId,
                Message=Message,
                UserName=context.LoginInfo.User.Name
            });

            Message=string.Empty; //发完消息就清除输入内容
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
                    if (!result.Items.Any()) return;

                    var list = Map<List<ChatMessageModel>>(result.Items);
                    userName = chatService.Friends
                    .FirstOrDefault(t => t.FriendUserId.Equals(list.First().TargetUserId)).FriendUserName;

                    foreach (var item in list)
                    {
                        UpdateMessageInfo(item);
                        Messages.Add(item);
                    }

                    await Task.CompletedTask;
                });
        }

        private void UpdateMessageInfo(ChatMessageModel model)
        {
            if (model.Side== ChatSide.Sender)
            {
                model.Color="#009933";
                model.UserName=context.LoginInfo.User.Name;
            }
            else
                model.UserName=userName;
        }
    }
}

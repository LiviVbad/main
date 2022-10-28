using AppFramework.Shared;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services.Signalr;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        private readonly IFriendChatService chatService;

        private FriendModel friend;

        public FriendModel Friend
        {
            get { return friend; }
            set { friend = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SendCommand { get; private set; }

        public FriendsChatViewModel(IFriendChatService chatService)
        {
            this.chatService=chatService;
            SendCommand=new DelegateCommand(Send);
        }

        private void Send()
        {
            chatService.SendMessage(new SendChatMessageInput()
            {
                UserId= Friend.FriendUserId,
            });
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
                Friend= parameters.GetValue<FriendModel>("Value");
        }
    }
}

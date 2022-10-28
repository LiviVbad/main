using Abp.Runtime.Security;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services.Signalr;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Services.Dialogs;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace AppFramework.ViewModels
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        public FriendsChatViewModel(IApplicationContext context,
           IChatAppService chatApp,
           IFriendChatService chatService,
           IProfileAppService profileAppService,
           IAccessTokenManager tokenManager,
           ProxyChatControllerService profileControllerService)
        {
            this.context=context;
            this.chatApp=chatApp;
            this.chatService=chatService;
            this.profileAppService=profileAppService;
            this.tokenManager=tokenManager;
            this.profileControllerService=profileControllerService;
            chatService.OnChatMessageHandler+=ChatService_OnChatMessageHandler;
            messages =new ObservableCollection<ChatMessageModel>();
            SendCommand =new DelegateCommand(Send);

            PickFileCommand=new DelegateCommand(PickFile);
            PickImageCommand =new DelegateCommand(PickImage);
        }

        #region 字段/属性

        private readonly IApplicationContext context;
        private readonly IChatAppService chatApp;
        private readonly IFriendChatService chatService;
        private readonly IProfileAppService profileAppService;
        private readonly IAccessTokenManager tokenManager;
        private readonly ProxyChatControllerService profileControllerService;
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
        public DelegateCommand PickImageCommand { get; private set; }
        public DelegateCommand PickFileCommand { get; private set; }

        #endregion

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

        public async void Send()
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

            if (model.Message.StartsWith("[image]"))
            {
                model.MessageType="image";

                var accessToken = tokenManager.GetAccessToken();
                var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);

                var msg = model.Message.Replace("[image]", "");
                var output = JsonConvert.DeserializeObject<ChatUploadFileOutput>(msg);
                model.Message=output.Name; 
                model.DownloadUrl=ApiUrlConfig.DefaultHostUrl+$"Chat/GetUploadedObject?fileId={output.Id}" +
                    $"&fileName={output.Name}" +
                    $"&contentType={output.ContentType}" +
                    $"&enc_auth_token={code}";
            }
            else if (model.Message.StartsWith("[file]"))
            {
                model.MessageType="file";

                var accessToken = tokenManager.GetAccessToken();
                var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);

                var msg = model.Message.Replace("[file]", "");
                var output = JsonConvert.DeserializeObject<ChatUploadFileOutput>(msg);
                model.Message=output.Name; //显示文件名
                model.DownloadUrl=ApiUrlConfig.DefaultHostUrl+$"Chat/GetUploadedObject?fileId={output.Id}" +
                   $"&fileName={output.Name}" +
                   $"&contentType={output.ContentType}" +
                   $"&enc_auth_token={code}";
            }
            else if (model.Message.StartsWith("[link]"))
            {
                model.MessageType="link";
                var msg = model.Message.Replace("[link]", "");
                model.Message = JsonConvert.DeserializeObject<dynamic>(msg).message;
            }
            else
            {
                model.MessageType="text";
            }
        }

        private void PickFile() { }

        private async void PickImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "图片文件(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            var dialogResult = fileDialog.ShowDialog();
            if ((bool)dialogResult)
            {
                string fileName = fileDialog.SafeFileName;
                string contentType = Path.GetExtension(fileName).Replace(".", "");
                var photoAsBytes = File.ReadAllBytes(fileDialog.FileName);

                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => UpdateProfilePhoto(photoAsBytes, fileName, $"images/{contentType}"),
                        UpdateProfileSuccessed);
                });
            }
        }

        private async Task<ChatUploadFileOutput> UpdateProfilePhoto(byte[] photoAsBytes, string fileName, string contentType)
        {
            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                return await profileControllerService.UploadFile(content =>
                 { 
                     content.AddFile("file", photoStream, fileName, contentType); 
                     content.AddString(nameof(FileDto.FileName), fileName); 
                 });
            }
        }

        private async Task UpdateProfileSuccessed(ChatUploadFileOutput output)
        {
            string message = $"[image]{{\"id\":\"{output.Id}\", " +
                $"\"name\":\"{output.Name}\", " +
                $"\"contentType\":\"{output.ContentType}\"}}";

            await chatService.SendMessage(new SendChatMessageInput()
            {
                UserId= Friend.FriendUserId,
                Message=message,
                UserName=context.LoginInfo.User.Name
            });
        }
    }
}

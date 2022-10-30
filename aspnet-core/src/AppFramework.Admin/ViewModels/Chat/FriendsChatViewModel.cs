using Abp.Runtime.Security;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Profile;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeMapping;
using Prism.Events;
using AppFramework.Admin.Events;
using System;

namespace AppFramework.Admin.ViewModels.Chat
{
    public class FriendsChatViewModel : HostDialogViewModel
    {
        public FriendsChatViewModel(IApplicationContext context,
           IChatAppService chatApp,
           IFriendChatService chatService,
           IProfileAppService profileAppService,
           IAccessTokenManager tokenManager,
           IEventAggregator aggregator,
           ProxyChatControllerService proxyChat)
        {
            this.context = context;
            this.chatApp = chatApp;
            this.chatService = chatService;
            this.profileAppService = profileAppService;
            this.tokenManager = tokenManager;
            this.aggregator = aggregator;
            this.proxyChat = proxyChat;
            chatService.OnChatMessageHandler += ChatService_OnChatMessageHandler;
            messages = new ObservableCollection<ChatMessageModel>();
            SendCommand = new DelegateCommand(Send);

            PickFileCommand = new DelegateCommand(PickFile);
            PickImageCommand = new DelegateCommand(PickImage);
            OpenFolderCommand = new DelegateCommand<ChatMessageModel>(OpenFolder);
        }

        #region 字段/属性

        private string userName;
        private string message;
        private FriendModel friend;
        private readonly IApplicationContext context;
        private readonly IChatAppService chatApp;
        private readonly IFriendChatService chatService;
        private readonly IProfileAppService profileAppService;
        private readonly IAccessTokenManager tokenManager;
        private readonly IEventAggregator aggregator;
        private readonly ProxyChatControllerService proxyChat;
        private ObservableCollection<ChatMessageModel> messages;

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public FriendModel Friend
        {
            get { return friend; }
            set { friend = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<ChatMessageModel> Messages
        {
            get { return messages; }
            set { messages = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SendCommand { get; private set; }
        public DelegateCommand PickImageCommand { get; private set; }
        public DelegateCommand PickFileCommand { get; private set; }
        public DelegateCommand<ChatMessageModel> OpenFolderCommand { get; private set; }

        #endregion

        #region 消息处理

        /// <summary>
        /// 加载用户的聊天记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task GetUserChatMessagesByUser(long userId)
        {
            await WebRequest.Execute(() =>
                chatApp.GetUserChatMessages(new GetUserChatMessagesInput()
                {
                    UserId = userId
                }), async result =>
                {
                    if (!result.Items.Any()) return;

                    var list = Map<List<ChatMessageModel>>(result.Items);
                    userName = chatService.Friends
                    .FirstOrDefault(t => t.FriendUserId.Equals(list.First().TargetUserId)).FriendUserName;

                    foreach (var item in list)
                    {
                        await UpdateMessageInfo(item);
                        Messages.Add(item);
                    }

                    aggregator.GetEvent<ScrollEvent>().Publish(true);
                    await MarkAllUnreadMessages();
                });
        }

        /// <summary>
        /// 更新消息格式
        /// </summary>
        /// <param name="model"></param>
        private async Task UpdateMessageInfo(ChatMessageModel model)
        {
            if (model.Side == ChatSide.Sender)
            {
                model.Color = "#009933";
                model.UserName = context.LoginInfo.User.Name;
            }
            else
                model.UserName = userName;

            if (model.Message.StartsWith("[image]"))
            {
                model.MessageType = "image";

                var accessToken = tokenManager.GetAccessToken();
                var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);

                var msg = model.Message.Replace("[image]", "");
                var output = JsonConvert.DeserializeObject<ChatUploadFileOutput>(msg);
                model.Message = output.Name; //显示文件名

                var downloadUrl = ApiUrlConfig.DefaultHostUrl + $"Chat/GetUploadedObject?fileId={output.Id}" +
                    $"&fileName={output.Name}" +
                    $"&contentType={output.ContentType}" +
                    $"&enc_auth_token={code}";

                await DownloadAsync(downloadUrl, AppConsts.DocumentPath, output.Name);
            }
            else if (model.Message.StartsWith("[file]"))
            {
                model.MessageType = "file";

                var accessToken = tokenManager.GetAccessToken();
                var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);

                var msg = model.Message.Replace("[file]", "");
                var output = JsonConvert.DeserializeObject<ChatUploadFileOutput>(msg);
                model.Message = output.Name; //显示文件名

                var downloadUrl = ApiUrlConfig.DefaultHostUrl + $"Chat/GetUploadedObject?fileId={output.Id}" +
                   $"&fileName={output.Name}" +
                   $"&contentType={output.ContentType}" +
                   $"&enc_auth_token={code}";

                await DownloadAsync(downloadUrl, AppConsts.DocumentPath, output.Name);
            }
            else if (model.Message.StartsWith("[link]"))
            {
                model.MessageType = "link";
                var msg = model.Message.Replace("[link]", "");
                model.Message = JsonConvert.DeserializeObject<dynamic>(msg).message;
            }
            else
            {
                model.MessageType = "text";
            }
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="chatMessage"></param>
        private async void ChatService_OnChatMessageHandler(ChatMessageDto chatMessage)
        {
            var msg = Map<ChatMessageModel>(chatMessage);
            await UpdateMessageInfo(msg);
            Messages.Add(msg);

            await MarkAllUnreadMessages();
        }

        /// <summary>
        /// 标记消息已读
        /// </summary>
        /// <returns></returns>
        private async Task MarkAllUnreadMessages()
        {
            await WebRequest.Execute(async () =>
            {
                await chatApp.MarkAllUnreadMessagesOfUserAsRead(new MarkAllUnreadMessagesOfUserAsReadInput()
                {
                    UserId = Friend.FriendUserId
                });
            });
        }

        #endregion

        #region 发送图片/文件

        private async Task DownloadAsync(string url, string localFolderPath, string fileName)
        {
            if (File.Exists($"{localFolderPath}{fileName}"))
            {
                await Task.CompletedTask;
            }
            else
            {
                await proxyChat.DownloadAsync(url, localFolderPath, fileName);
            }
        }

        private void OpenFolder(ChatMessageModel msg)
        {
            if (Directory.Exists(AppConsts.DocumentPath))
                System.Diagnostics.Process.Start("explorer.exe", AppConsts.DocumentPath);
        }

        private async void PickFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所有文件(*.*)|*.*";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult!=null&&(bool)dialogResult)
            {
                string fileName = fileDialog.SafeFileName;
                string contentType = MimeUtility.GetMimeMapping(fileName);
                var photoAsBytes = File.ReadAllBytes(fileDialog.FileName);

                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => UploadFile(photoAsBytes, fileName, contentType),
                        async output =>
                        {
                            string message = $"[file]{{\"id\":\"{output.Id}\", " +
                                             $"\"name\":\"{output.Name}\", " +
                                             $"\"contentType\":\"{output.ContentType}\"}}";

                            await chatService.SendMessage(new SendChatMessageInput()
                            {
                                UserId = Friend.FriendUserId,
                                Message = message,
                                UserName = context.LoginInfo.User.Name
                            });
                        });
                });
            }
        }

        private async void PickImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "图片文件(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            var dialogResult = fileDialog.ShowDialog();
            if ((bool)dialogResult)
            {
                string fileName = fileDialog.SafeFileName;
                string contentType = MimeUtility.GetMimeMapping(fileName);
                var photoAsBytes = File.ReadAllBytes(fileDialog.FileName);

                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => UploadFile(photoAsBytes, fileName, contentType),
                        async output =>
                        {
                            string message = $"[image]{{\"id\":\"{output.Id}\", " +
                                             $"\"name\":\"{output.Name}\", " +
                                             $"\"contentType\":\"{output.ContentType}\"}}";

                            await chatService.SendMessage(new SendChatMessageInput()
                            {
                                UserId = Friend.FriendUserId,
                                Message = message,
                                UserName = context.LoginInfo.User.Name
                            });
                        });
                });
            }
        }

        private async Task<ChatUploadFileOutput> UploadFile(byte[] photoAsBytes, string fileName, string contentType)
        {
            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                return await proxyChat.UploadFile(content =>
                 {
                     content.AddFile("file", photoStream, fileName, contentType);
                     content.AddString(nameof(FileDto.FileName), fileName);
                 });
            }
        }

        #endregion

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public override void Cancel()
        {
            chatService.OnChatMessageHandler -= ChatService_OnChatMessageHandler;
            base.Cancel();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public async void Send()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;

            await chatService.SendMessage(new SendChatMessageInput()
            {
                UserId = Friend.FriendUserId,
                Message = Message,
                UserName = context.LoginInfo.User.Name
            });

            Message = string.Empty; //发完消息就清除输入内容
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Friend = parameters.GetValue<FriendModel>("Value");

                await GetUserChatMessagesByUser(Friend.FriendUserId);
            }
        }
    }
}

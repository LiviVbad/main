using Abp;
using Abp.Runtime.Security;
using AppFramework.ApiClient;
using AppFramework.Chat;
using AppFramework.Chat.Dto;
using AppFramework.Friendships;
using AppFramework.Friendships.Dto;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Signalr
{
    public class FriendChatService : 
        ViewModelBase, IFriendChatService
    {
        public FriendChatService(IAccessTokenManager context,
            IFriendshipAppService friendshipAppService,
            IChatAppService chatAppService)
        {
            this.context=context;
            this.friendshipAppService=friendshipAppService;
            this.chatAppService=chatAppService;
            friends=new ObservableCollection<FriendModel>();
        }

        public event DelegateChatMessageHandler OnChatMessageHandler;
        private HubConnection signalr;
        private HubConnection signalrChat;
        private readonly IAccessTokenManager context;
        private readonly IFriendshipAppService friendshipAppService;
        private readonly IChatAppService chatAppService;

        public bool IsConnected { get; private set; }

        private ObservableCollection<FriendModel> friends;

        public ObservableCollection<FriendModel> Friends
        {
            get { return friends; }
            set { friends = value; RaisePropertyChanged(); }
        }

        public async Task GetUserChatFriendsAsync()
        {
            await SetBusyAsync(async () =>
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
                                item.FriendTenancyName="Host";

                            Friends.Add(item);
                        }
                    }
                });
            });
        }

        #region Chat

        public async Task SendMessage(SendChatMessageInput input)
        {
            if (!IsConnected)
                throw new Exception("Please try again after connecting to the server!");

            if (input==null)
                throw new ArgumentNullException(nameof(input));

            await signalrChat.InvokeAsync("sendMessage", input);
        }

        public async Task StartAsync()
        {
            if (IsConnected) return;

            await SignalrConnect();
            await SignalrChatConnect();
            IsConnected =true;
        }

        public async Task StopAsync()
        {
            await signalr.StopAsync();
            await signalrChat.StopAsync();
            IsConnected=false;
        }

        private async Task SignalrConnect()
        {
            var accessToken = context.GetAccessToken();
            var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
            var token = $"/?enc_auth_token={code}";
            signalr = new HubConnectionBuilder()
                   .WithUrl(ApiUrlConfig.DefaultHostUrl+"signalr"+token, Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets)
                   .Build();

            signalr.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await signalr.StartAsync();
            };

            await signalr.StartAsync();
        }

        private async Task SignalrChatConnect()
        {
            var accessToken = context.GetAccessToken();
            var code = SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
            var token = $"/?enc_auth_token={code}";
            signalrChat = new HubConnectionBuilder()
                   .WithUrl(ApiUrlConfig.DefaultHostUrl+"signalr-chat"+token, Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets)
                   .Build();

            signalrChat.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await signalrChat.StartAsync();
            };

            signalrChat.On<ChatMessageDto>("getChatMessage", GetChatMessageHandler);
            signalrChat.On<FriendDto, bool>("getFriendshipRequest", GetFriendshipRequestHandler);
            signalrChat.On<UserIdentifier, bool>("getUserConnectNotification", GetUserConnectNotificationHandler);
            signalrChat.On<UserIdentifier, FriendshipState>("getUserStateChange", GetUserStateChangeHandler);
            signalrChat.On<UserIdentifier>("getallUnreadMessagesOfUserRead", GetallUnreadMessagesOfUserReadHandler);
            signalrChat.On<UserIdentifier>("getReadStateChange", GetReadStateChangeHandler);
            await signalrChat.StartAsync();
        }

        #endregion

        #region ChatHandler

        /// <summary>
        /// 回调聊天消息
        /// </summary>
        /// <param name="message"></param>
        private void GetChatMessageHandler(ChatMessageDto message)
        {
            if (OnChatMessageHandler!=null)
            {
                OnChatMessageHandler.Invoke(message);
            }
            else
            {
                var friend = Friends.FirstOrDefault(t => t.FriendUserId.Equals(message.TargetUserId));
                if (friend!=null)
                    friend.UnreadMessageCount+=1;
            }
        }

        /// <summary>
        /// 好友请求
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="result"></param>
        private void GetFriendshipRequestHandler(FriendDto friend, bool result)
        {

        }

        /// <summary>
        /// 用户上线通知
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="isConnected"></param>
        private void GetUserConnectNotificationHandler(UserIdentifier friend, bool isConnected)
        {
            var friendUser = Friends.FirstOrDefault(t => t.FriendUserId.Equals(friend.UserId));
            if (friendUser!=null)
                friendUser.IsOnline=isConnected;
        }

        /// <summary>
        /// 用户状态改变
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="state"></param>
        private void GetUserStateChangeHandler(UserIdentifier friend, FriendshipState state)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        private void GetallUnreadMessagesOfUserReadHandler(UserIdentifier user)
        {

        }
         
        private void GetReadStateChangeHandler(UserIdentifier user)
        {
           //如果要处理消息得已读状态,可以订阅这里
        }

        #endregion
    }
}

using Abp;
using AppFramework.ApiClient;
using AppFramework.Chat.Dto;
using AppFramework.Friendships;
using AppFramework.Friendships.Dto;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks; 

namespace AppFramework.Shared.Services.Signalr
{
    public class SignalrService : ISignalrService
    {
        HubConnection service;

        public bool IsConnected { get; private set; }

        public SignalrService()
        {
            service = new HubConnectionBuilder()
                .WithUrl(ApiUrlConfig.DefaultHostUrl+"signalr", option =>
                {  
                })
                .Build();
             
            service.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await service.StartAsync();
            };

            service.On<ChatMessageDto>("getChatMessage", GetChatMessageHandler);
            //service.On<FriendDto[]>("getAllFriends", GetAllFriendsHandler);
            service.On<FriendDto, bool>("getFriendshipRequest", GetFriendshipRequestHandler);
            service.On<UserIdentifier, bool>("getUserConnectNotification", GetUserConnectNotificationHandler);
            service.On<UserIdentifier, FriendshipState>("getUserStateChange", GetUserStateChangeHandler);
            service.On<UserIdentifier>("getallUnreadMessagesOfUserRead", GetallUnreadMessagesOfUserReadHandler);
            service.On<UserIdentifier>("getReadStateChange", GetReadStateChangeHandler);
        }

        public async Task SendMessage(SendChatMessageInput input)
        {
            if (!IsConnected)
                throw new Exception("Please try again after connecting to the server!");

            if (input==null)
                throw new ArgumentNullException(nameof(input));

            await service.InvokeAsync("sendMessage", input);
        }

        public async Task StartAsync()
        {
            await service.StartAsync();
            IsConnected=true;
        }

        public async Task StopAsync()
        {
            await service.StopAsync();
            IsConnected=false;
        }

        private void GetChatMessageHandler(ChatMessageDto message)
        {

        }

        private void GetAllFriendsHandler(FriendDto[] friends)
        {

        }

        private void GetFriendshipRequestHandler(FriendDto friend, bool result)
        {

        }

        private void GetUserConnectNotificationHandler(UserIdentifier friend, bool isConnected)
        {
        }

        private void GetUserStateChangeHandler(UserIdentifier friend, FriendshipState state)
        {

        }

        private void GetallUnreadMessagesOfUserReadHandler(UserIdentifier user)
        {

        }

        private void GetReadStateChangeHandler(UserIdentifier user)
        {

        }
    }
}

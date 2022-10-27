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
                .WithUrl(ApiUrlConfig.DefaultHostUrl)
                .Build();

            service.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await service.StartAsync();
            };

            service.On<ChatMessageDto>("getChatMessage", message => { });
            service.On<object>("getAllFriends", friends => { });
            service.On<FriendDto, bool>("getFriendshipRequest", (friendData, isOwnRequest) => { });
            service.On<UserIdentifier, bool>("getUserConnectNotification", (friend, isConnected) => { });
            service.On<UserIdentifier, FriendshipState>("getUserStateChange", (friend, state) => { });
            service.On<UserIdentifier>("getallUnreadMessagesOfUserRead", user => { });
            service.On<UserIdentifier>("getReadStateChange", user => { });
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
    }
}

﻿using AppFramework.Chat.Dto;
using AppFramework.Shared.Models.Chat;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Signalr
{
    public delegate void DelegateChatMessageHandler(ChatMessageDto chatMessage);

    public interface IFriendChatService
    {
        event DelegateChatMessageHandler OnChatMessageHandler;

        bool IsConnected { get; }

        Task StartAsync();

        Task StopAsync();

        Task SendMessage(SendChatMessageInput input);

        Task GetUserChatFriendsAsync();

        ObservableCollection<FriendModel> Friends { get; set; }
    }

    public class SendChatMessageInput
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public string TenancyName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public string Message { get; set; }
    }
}
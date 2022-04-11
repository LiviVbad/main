using AppFrameworkDemo.Friendships.Dto;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;

namespace AppFrameworkDemo.Chat.Dto
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }

        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}
using AppFramework.Friendships;
using Prism.Mvvm;
using System; 

namespace Models.Chat
{
    public class FriendModel : BindableBase
    {
        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public Guid? FriendProfilePictureId { get; set; }

        public int UnreadMessageCount { get; set; }

        public bool IsOnline { get; set; }

        public FriendshipState State { get; set; }
    }
}

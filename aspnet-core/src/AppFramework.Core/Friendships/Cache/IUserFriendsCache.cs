﻿using Abp;
using System.Threading.Tasks;

namespace AppFramework.Friendships.Cache
{
    public interface IUserFriendsCache
    {
        Task<UserWithFriendsCacheItem> GetCacheItem(UserIdentifier userIdentifier);

        UserWithFriendsCacheItem GetCacheItemOrNull(UserIdentifier userIdentifier);

        void ResetUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier);

        void IncreaseUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier, int change);

        void AddFriend(UserIdentifier userIdentifier, FriendCacheItem friend);

        void RemoveFriend(UserIdentifier userIdentifier, FriendCacheItem friend);

        void UpdateFriend(UserIdentifier userIdentifier, FriendCacheItem friend);
    }
}

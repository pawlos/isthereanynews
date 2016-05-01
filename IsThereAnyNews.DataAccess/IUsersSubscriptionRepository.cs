using System;
using System.Collections.Generic;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    public interface IUsersSubscriptionRepository
    {
        void CreateNewSubscription(long followerId, long observedId);
        List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId);
        void UpdateUserLastReadTime(long currentUserId, DateTime now);
        bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId);
        List<UserSubscriptionEntryToRead> LoadAllUnreadEntriesFromSubscription(long subscriptionId);
        RssChannelSubscription LoadChannelInformation(long subscriptionId);
    }

}
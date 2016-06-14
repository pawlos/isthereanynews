using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    public interface IPersonSubscriptionRepository
    {
        bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId);
        List<RssEntryToRead> LoadAllUnreadEntriesFromSubscription(long subscriptionId);
        RssChannelSubscription LoadChannelInformation(long subscriptionId);
    }
}
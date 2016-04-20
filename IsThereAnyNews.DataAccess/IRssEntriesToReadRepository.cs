using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssEntriesToReadRepository
    {
        void CopyRssThatWerePublishedAfterLastReadTimeToUser(long currentUserId, List<RssChannelSubscription> rssChannelsIds);
        void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> rssId);
        List<RssEntryToRead> LoadAllUnreadEntriesFromSubscription(long subscriptionId);
        void MarkEntryViewedByUser(long currentUserId, long rssToReadId);
    }
}
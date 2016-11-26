namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public interface IRssEntriesToReadRepository
    {
        void CopyRssThatWerePublishedAfterLastReadTimeToUser(long currentUserId, List<RssChannelSubscriptionDTO> rssChannelsIds);
        void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> rssId);
        List<RssEntryToRead> LoadAllUnreadEntriesFromSubscription(long subscriptionId);
        void MarkEntryViewedByUser(long currentUserId, long rssToReadId);
        List<RssEntryToRead> LoadAllEntriesFromSubscription(long subscriptionId);
    }
}
namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;

    public interface IRssEntriesToReadRepository
    {
        void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> rssId);
        List<RssEntryToReadDTO> LoadAllUnreadEntriesFromSubscription(long subscriptionId);
        void MarkEntryViewedByUser(long currentUserId, long rssToReadId);
        List<RssEntryToReadDTO> LoadAllEntriesFromSubscription(long subscriptionId);
        void MarkEntriesSkipped(long modelSubscriptionId, List<long> ids);

        List<RssEntryDTO> LoadRss(long subscriptionId, ShowReadEntries showReadEntries);

        void InsertReadRssToRead(long userId, long rssId, long dtoSubscriptionId);
    }
}
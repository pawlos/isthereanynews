using System;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelWithStatisticsViewModel
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public int RssEntriesCount { get; set; }
        public int SubscriptionsCount { get; set; }
        public string Title { get; set; }
        public DateTime Updated { get; set; }

        public RssChannelWithStatisticsViewModel(long id, DateTime created, int rssEntriesCount, int subscriptionsCount, string title, DateTime updated)
        {
            Id = id;
            Created = created;
            RssEntriesCount = rssEntriesCount;
            SubscriptionsCount = subscriptionsCount;
            Title = title;
            Updated = updated;
        }
    }
}
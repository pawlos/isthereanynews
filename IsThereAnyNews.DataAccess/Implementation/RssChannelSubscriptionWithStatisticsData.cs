using System;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelSubscriptionWithStatisticsData
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int SubscriptionsCount { get; set; }
        public int RssEntriesCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
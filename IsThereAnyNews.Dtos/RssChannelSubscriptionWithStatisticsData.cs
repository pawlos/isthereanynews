namespace IsThereAnyNews.Dtos
{
    using System;

    public class RssChannelSubscriptionWithStatisticsData
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int RssEntriesCount { get; set; }
        public DateTime Created { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
using System;

namespace IsThereAnyNews.ViewModels.RssChannel
{
    public class RssChannelWithStatisticsViewModel
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public int RssEntriesCount { get; set; }
        public int SubscriptionsCount { get; set; }
        public string Title { get; set; }
        public DateTime Updated { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
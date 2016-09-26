namespace IsThereAnyNews.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class RssChannelIndexViewModel
    {
        public string Title { get; set; }
        public DateTime Added { get; set; }
        public bool IsAuthenticatedUser { get; set; }
        public UserRssSubscriptionInfoViewModel SubscriptionInfo { get; set; }
        public long ChannelId { get; set; }
        public List<RssEntryViewModel> Entries { get; set; }
    }
}
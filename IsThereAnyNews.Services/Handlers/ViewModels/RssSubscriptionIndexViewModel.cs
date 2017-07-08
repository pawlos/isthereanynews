namespace IsThereAnyNews.Services.Handlers.ViewModels
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class RssSubscriptionIndexViewModel: ISubscriptionContentIndexViewModel
    {
        public RssSubscriptionIndexViewModel(long subscriptionId, string title, DateTime creationDateTime, List<RssEntryToReadViewModel> loadAllRssEntriesForUserAndChannel)
        {
            this.SubscriptionId = subscriptionId;
            this.Title = title;
            this.CreationDateTime = creationDateTime;
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel;
        }

        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        public StreamType StreamType => StreamType.Rss;
        public long SubscriptionId { get; }
        public string Title { get; }
        public DateTime CreationDateTime { get; }
    }
}
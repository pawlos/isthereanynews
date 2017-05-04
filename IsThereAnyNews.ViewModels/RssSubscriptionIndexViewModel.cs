using System;

namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.SharedData;

    public class RssSubscriptionIndexViewModel
    {
        public RssSubscriptionIndexViewModel(long subscriptionId, string title, DateTime creationDateTime, List<RssEntryToReadViewModel> loadAllRssEntriesForUserAndChannel, StreamType streamType)
        {
            this.SubscriptionId = subscriptionId;
            Title = title;
            CreationDateTime = creationDateTime;
            this.StreamType = streamType;
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel;
        }

        public string DisplayedRss => string.Join(";", this.RssEntryToReadViewModels.Select(x => x.RssEntryViewModel.Id));

        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; set; }

        public StreamType StreamType { get; set; }

        public long SubscriptionId { get; set; }
        public string Title { get; }
        public DateTime CreationDateTime { get; }
    }
}
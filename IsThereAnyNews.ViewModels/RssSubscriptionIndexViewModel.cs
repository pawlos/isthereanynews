﻿using System;

namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.SharedData;

    public class RssSubscriptionIndexViewModel: ISubscriptionContentIndexViewModel
    {
        public RssSubscriptionIndexViewModel(long subscriptionId, string title, DateTime creationDateTime, List<RssEntryToReadViewModel> loadAllRssEntriesForUserAndChannel)
        {
            this.SubscriptionId = subscriptionId;
            this.Title = title;
            this.CreationDateTime = creationDateTime;
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel;
        }

        public List<long> DisplayedRss => this.RssEntryToReadViewModels.Select(x => x.RssEntryViewModel.Id).ToList();
        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        public StreamType StreamType => StreamType.Rss;
        public long SubscriptionId { get; }
        public string Title { get; }
        public DateTime CreationDateTime { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Handlers.Implementation
{
    public class ExceptionSubscriptionIndexViewModel : ISubscriptionContentIndexViewModel
    {
        public ExceptionSubscriptionIndexViewModel(long subscriptionId, string title, DateTime creationDateTime, List<RssEntryToReadViewModel> rssEntryToReadViewModels)
        {
            SubscriptionId = subscriptionId;
            Title = title;
            CreationDateTime = creationDateTime;
            RssEntryToReadViewModels = rssEntryToReadViewModels;
        }

        public List<long> DisplayedRss => RssEntryToReadViewModels.Select(x => x.Id).ToList();
        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        public StreamType StreamType =>StreamType.Exception;
        public long SubscriptionId { get; }
        public string Title { get; }
        public DateTime CreationDateTime { get; }
    }
}
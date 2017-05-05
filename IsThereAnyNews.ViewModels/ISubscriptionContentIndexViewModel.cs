using System;
using System.Collections.Generic;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels
{
    public interface ISubscriptionContentIndexViewModel
    {
        List<long> DisplayedRss { get; }
        List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        StreamType StreamType { get; }
        long SubscriptionId { get; }
        string Title { get; }
        DateTime CreationDateTime { get; }
    }
}
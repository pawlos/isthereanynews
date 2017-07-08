namespace IsThereAnyNews.ViewModels
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.SharedData;

    public interface ISubscriptionContentIndexViewModel
    {
        List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        StreamType StreamType { get; }
        long SubscriptionId { get; }
        string Title { get; }
        DateTime CreationDateTime { get; }
    }
}
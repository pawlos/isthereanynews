namespace IsThereAnyNews.Services.Handlers.ViewModels
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class ExceptionSubscriptionIndexViewModel : ISubscriptionContentIndexViewModel
    {
        public ExceptionSubscriptionIndexViewModel(long subscriptionId, string title, DateTime creationDateTime, List<RssEntryToReadViewModel> rssEntryToReadViewModels)
        {
            this.SubscriptionId = subscriptionId;
            this.Title = title;
            this.CreationDateTime = creationDateTime;
            this.RssEntryToReadViewModels = rssEntryToReadViewModels;
        }

        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        public StreamType StreamType =>StreamType.Exception;
        public long SubscriptionId { get; }
        public string Title { get; }
        public DateTime CreationDateTime { get; }
    }
}
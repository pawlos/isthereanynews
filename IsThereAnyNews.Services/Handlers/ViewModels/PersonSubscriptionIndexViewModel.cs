namespace IsThereAnyNews.Services.Handlers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class PersonSubscriptionIndexViewModel: ISubscriptionContentIndexViewModel
    {
        public PersonSubscriptionIndexViewModel(long subscriptionId, string title, DateTime creationDateTime, List<RssEntryToReadViewModel> rssEntryToReadViewModels)
        {
            this.SubscriptionId = subscriptionId;
            this.Title = title;
            this.CreationDateTime = creationDateTime;
            this.RssEntryToReadViewModels = rssEntryToReadViewModels;
        }

        public List<long> DisplayedRss => this.RssEntryToReadViewModels.Select(x => x.Id).ToList();
        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; }
        public StreamType StreamType => StreamType.Person;
        public long SubscriptionId { get; }
        public string Title { get; }
        public DateTime CreationDateTime { get; }
    }
}
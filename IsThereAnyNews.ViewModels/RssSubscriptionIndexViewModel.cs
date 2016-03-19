using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.ViewModels
{
    public class RssSubscriptionIndexViewModel
    {
        public RssSubscriptionIndexViewModel(RssChannelSubscription loadAllRssEntriesForUserAndChannel)
        {
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel.RssEntriesToRead.Select(r => new RssEntryToReadViewModel(r)).ToList();
        }

        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; set; }
        public long SubscriptionId { get; set; }
        public string DisplayedRss => string.Join(";", RssEntryToReadViewModels.Select(x => x.Id));
    }
}
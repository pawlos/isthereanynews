using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.ViewModels
{
    public class RssSubscriptionIndexViewModel
    {
        public RssSubscriptionIndexViewModel(List<RssEntryToRead> loadAllRssEntriesForUserAndChannel)
        {
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel.Select(r => new RssEntryToReadViewModel(r)).ToList();
        }

        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; set; }
        public long SubscriptionId { get; set; }
        public string DisplayedRss => string.Join(";", RssEntryToReadViewModels.Select(x => x.Id));
    }
}
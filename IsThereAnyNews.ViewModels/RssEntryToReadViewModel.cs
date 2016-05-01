using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.ViewModels
{
    public class RssEntryToReadViewModel
    {
        public RssEntryToReadViewModel(RssEntryToRead rssEntryToRead)
        {
            this.Id = rssEntryToRead.Id;
            this.IsRead = rssEntryToRead.IsRead;
            this.RssEntryViewModel = new RssEntryViewModel(rssEntryToRead.RssEntry);
        }

        public RssEntryToReadViewModel(UserSubscriptionEntryToRead rssEntryToRead)
        {
            this.Id = rssEntryToRead.Id;
            this.IsRead = false;
            this.RssEntryViewModel = new RssEntryViewModel(rssEntryToRead.EventRssViewed.RssEntry);
        }

        public RssEntryViewModel RssEntryViewModel { get; set; }

        public bool IsRead { get; set; }

        public long Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelSubscriptionViewModel
    {
        public RssChannelSubscriptionViewModel(RssChannelSubscription subscription)
        {
            this.Id = subscription.Id;
            this.Title = subscription.Title;
            this.RssToRead = subscription.RssEntriesToRead.Select(x => new RssEntryToReadViewModel(x)).ToList();
        }

        public List<RssEntryToReadViewModel> RssToRead { get; set; }

        public string ChannelUrl { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }
    }

    public class RssEntryToReadViewModel
    {
        public RssEntryToReadViewModel(RssEntryToRead rssEntryToRead)
        {
            this.Id = rssEntryToRead.Id;
            this.IsRead = rssEntryToRead.IsRead;
            this.RssEntryViewModel = new RssEntryViewModel(rssEntryToRead.RssEntry);
        }

        public RssEntryViewModel RssEntryViewModel { get; set; }

        public bool IsRead { get; set; }

        public long Id { get; set; }
    }

    public class RssEntryViewModel
    {
        public RssEntryViewModel(RssEntry rssEntry)
        {
            this.Id = rssEntry.Id;
            this.PreviewText = rssEntry.PreviewText;
            this.PublicationDate = rssEntry.PublicationDate;
            this.Title = rssEntry.Title;
        }

        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PreviewText { get; set; }
        public long Id { get; set; }
    }
}
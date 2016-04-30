using System;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.ViewModels
{
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
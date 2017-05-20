namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class FeedEntriesViewModel
    {
        public List<RssEntryViewModel> RssEntryViewModels { get; set; }
        public long EntriesCount => this.RssEntryViewModels.Count;
    }
}
namespace IsThereAnyNews.ViewModels
{
    using System;

    public class RssEntryViewModel
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PreviewText { get; set; }
        public long Id { get; set; }
        public string Url { get; set; }
    }
}
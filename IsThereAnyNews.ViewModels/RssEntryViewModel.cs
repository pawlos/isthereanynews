namespace IsThereAnyNews.ViewModels
{
    using System;

    public class RssEntryViewModel
    {
        public long Id { get; set; }

        public string PreviewText { get; set; }

        public DateTime PublicationDate { get; set; }

        public long SubscriptionId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsRead { get; set; }
    }
}
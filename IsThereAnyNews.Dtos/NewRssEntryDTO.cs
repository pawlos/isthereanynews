namespace IsThereAnyNews.Dtos
{
    using System;

    public class NewRssEntryDTO
    {
        public string GetContentOnly { get; set; }
        public string ItemId { get; set; }
        public DateTime ItemPublishDate { get; set; }
        public string ItemSummary { get; set; }
        public string ItemTitle { get; set; }
        public string ItemUrl { get; set; }
        public long RssChannelId { get; set; }
    }
}
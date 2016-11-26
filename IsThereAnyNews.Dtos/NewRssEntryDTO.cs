namespace IsThereAnyNews.Dtos
{
    using System;

    public class NewRssEntryDTO
    {
        public NewRssEntryDTO(string itemId, DateTimeOffset itemPublishDate, string itemTitle, string itemSummary, string getContentOnly, long rssChannelId, string itemUrl)
        {
            this.ItemId = itemId;
            this.ItemPublishDate = itemPublishDate;
            this.ItemTitle = itemTitle;
            this.ItemSummary = itemSummary;
            this.GetContentOnly = getContentOnly;
            this.RssChannelId = rssChannelId;
            this.ItemUrl = itemUrl;
        }

        public string GetContentOnly { get; set; }
        public string ItemId { get; set; }

        public DateTimeOffset ItemPublishDate { get; set; }

        public string ItemSummary { get; set; }
        public string ItemTitle { get; set; }
        public string ItemUrl { get; set; }
        public long RssChannelId { get; set; }
    }
}
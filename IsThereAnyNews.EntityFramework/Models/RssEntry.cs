using System;

namespace IsThereAnyNews.EntityFramework.Models
{
    public class RssEntry : IModel
    {
        public RssEntry() : this(string.Empty, DateTimeOffset.MinValue, string.Empty, string.Empty, 0)
        {

        }

        public RssEntry(string id,
                        DateTimeOffset publishDate,
                        string title,
                        string text,
                        long channelId)
        {
            this.RssId = id;
            this.PublicationDate = publishDate.DateTime;
            this.Title = title;
            this.PreviewText = text;
            this.RssChannelId = channelId;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public long RssChannelId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
        public string PreviewText { get; set; }
        public string RssId { get; set; }

        virtual public RssEntryToRead RssEntryToRead { get; set; }
    }
}
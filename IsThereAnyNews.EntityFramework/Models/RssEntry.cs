using System;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class RssEntry : IModel
    {
        public RssEntry(string id,
                        DateTimeOffset publishDate,
                        string title,
                        string text,
                        long l)
        {
            this.RssId = id;
            this.PublicationDate = publishDate.DateTime;
            this.Title = title;
            this.PreviewText = text;
            this.RssChannelId = l;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public long RssChannelId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
        public string PreviewText { get; set; }
        public string RssId { get; set; }
    }
}
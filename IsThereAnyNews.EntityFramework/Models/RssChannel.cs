using System;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class RssChannel : IModel
    {
        public RssChannel() : this(string.Empty, string.Empty)
        {
        }

        public RssChannel(string url, string title)
        {
            this.Url = url;
            this.Title = title;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTimeOffset RssLastUpdatedTime { get; set; }
    }
}
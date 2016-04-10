using System;
using System.Collections.Generic;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class RssChannel : IModel,IEqualityComparer<RssChannel>
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

        public List<RssChannelSubscription> Subscriptions { get; set; }
        public List<RssEntry> RssEntries { get; set; }

        public bool Equals(RssChannel x, RssChannel y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(RssChannel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
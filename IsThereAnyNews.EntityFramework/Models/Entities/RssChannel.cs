namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public sealed class RssChannel: IEntity, ICreatable, IModifiable, IEqualityComparer<RssChannel>
    {
        public RssChannel() : this(string.Empty, string.Empty, 0)
        {
        }

        public RssChannel(string url, string title, long submitterId)
        {
            this.Url = url;
            this.Title = title;
            this.SubmitterId = submitterId;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Title { get; set; }
        public long SubmitterId { get; set; }
        public string Url { get; set; }
        public DateTimeOffset RssLastUpdatedTime { get; set; }

        public List<RssChannelSubscription> Subscriptions { get; set; }
        public List<RssEntry> RssEntries { get; set; }

        public List<EventRssChannelUpdated> Updates { get; set; }

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
namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public sealed class RssChannelSubscription : IEntity, ICreatable, IModifiable
    {
        public RssChannelSubscription() : this(0, 0, string.Empty)
        {
        }

        public RssChannelSubscription(long rssChannelId, long subscriberId, string title)
        {
            this.RssChannelId = rssChannelId;
            this.UserId = subscriberId;
            this.Title = title;
            this.RssEntriesToRead = new List<RssEntryToRead>();
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string Title { get; set; }

        public User Subscriber { get; set; }
        public long UserId { get; set; }

        public RssChannel RssChannel { get; set; }
        public long RssChannelId { get; set; }

        public List<RssEntryToRead> RssEntriesToRead { get; set; }
    }
}
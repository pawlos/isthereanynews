﻿using System;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class RssChannelSubscription : IModel
    {
        public RssChannelSubscription() : this(0, 0, string.Empty)
        {

        }

        public RssChannelSubscription(long rssChannelId, long subscriberId, string title)
        {
            this.RssChannelId = rssChannelId;
            this.UserId = subscriberId;
            this.Title = title;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public long RssChannelId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }

        public User Subscriber { get; set; }
        public RssChannel RssChannel { get; set; }
    }
}
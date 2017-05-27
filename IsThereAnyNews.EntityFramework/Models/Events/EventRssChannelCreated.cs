using System;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Events
{
    public class EventRssChannelCreated : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }

        public long RssChannelId { get; set; }
        public RssChannel RssChannel { get; set; }

        public long SubmitterId { get; set; }
    }
}
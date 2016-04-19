using System;
using System.ComponentModel.DataAnnotations.Schema;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Events
{
    [Table("EventRssChannelUpdates")]
    public class EventRssChannelUpdated : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }

        public long RssChannelId { get; set; }
        public RssChannel RssChannel { get; set; }
    }
}
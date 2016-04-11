using System;
using System.Data.SqlTypes;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Events
{
    public class EventRssViewed : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }

        public long UserId { get; set; }
        public long RssEntryId { get; set; }

        public User User { get; set; }
        public RssEntry RssEntry { get; set; }
    }
}
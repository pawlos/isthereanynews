using System;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Events
{
    public class EventRssClicked : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }

        public long UserId { get; set; }
        public long RssEntryId { get; set; }

        public User User { get; set; }
        public RssEntry RssEntry { get; set; }
    }
}
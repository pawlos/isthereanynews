namespace IsThereAnyNews.EntityFramework.Models.Events
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class EventRssUserInteraction : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public long UserId { get; set; }
        public long RssEntryId { get; set; }
        public User User { get; set; }
        public RssEntry RssEntry { get; set; }
        public InteractionType InteractionType { get; set; }
    }
}
namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class EventRssChannelUpdatedToRead: IEntity, ICreatable, IModifiable
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsViewed { get; set; }
        public bool IsSkipped { get; set; }

        public long EventRssChannelUpdatedId { get; set; }
        public EventRssChannelUpdated EventRssChannelUpdated { get; set; }
    }
}
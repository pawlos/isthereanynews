using System;
using IsThereAnyNews.EntityFramework.Models.Events;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    public class UserSubscriptionEntryToRead : IEntity, ICreatable, IModifiable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public long EventRssViewedId { get; set; }
        public EventRssViewed EventRssViewed { get; set; }
    }
}
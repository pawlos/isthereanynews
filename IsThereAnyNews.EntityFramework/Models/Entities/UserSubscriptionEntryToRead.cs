namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class UserSubscriptionEntryToRead : IEntity, ICreatable, IModifiable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public long EventRssUserInteractionId { get; set; }
        public EventRssUserInteraction EventRssUserInteraction { get; set; }

        public long UserSubscriptionId { get; set; }
        public UserSubscription Subscription { get; set; }

        public bool IsRead { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsViewed { get; set; }
    }
}
namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Events;

    public class RssEventRepository : IRssEventRepository
    {
        private readonly ItanDatabaseContext database;

        public RssEventRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void AddEventRssViewed(long currentUserId, long rssToReadId)
        {
            var eventRssViewed = new EventRssUserInteraction
            {
                RssEntryId = rssToReadId,
                UserId = currentUserId,
                InteractionType = InteractionType.Viewed
            };

            this.database.EventsRssUserInteraction.Add(eventRssViewed);
            this.database.SaveChanges();
        }

        public void MarkRead(List<long> ids)
        {
            this.database.UsersSubscriptionsToRead
                .Where(x => ids.Contains(x.Id))
                .ToList()
                .ForEach(x => x.IsRead = true);
            this.database.SaveChanges();
        }

        public void MarkClicked(long id, long currentUserId)
        {
            var eventRssClicked = new EventRssUserInteraction()
            {
                UserId = currentUserId,
                RssEntryId = id,
                InteractionType = InteractionType.Clicked
            };

            this.database.EventsRssUserInteraction.Add(eventRssClicked);
            this.database.SaveChanges();
        }

        public void AddEventRssSkipped(long cui, List<long> ids)
        {
            var eventsToSave = ids.Select(id =>
                new EventRssUserInteraction
                {
                    UserId = cui,
                    RssEntryId = id,
                    InteractionType = InteractionType.Skipped
                });

            this.database.EventsRssUserInteraction.AddRange(eventsToSave);
            this.database.SaveChanges();
        }
    }
}
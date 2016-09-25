using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssEventRepository : IRssEventRepository
    {
        private readonly ItanDatabaseContext database;

        public RssEventRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void AddEventRssViewed(long currentUserId, long rssToReadId)
        {
            var rssEntryId = this.database.RssEntriesToRead.Single(r => r.Id == rssToReadId).RssEntryId;
            var eventRssViewed = new EventRssUserInteraction
            {
                RssEntryId = rssEntryId,
                UserId = currentUserId,
                InteractionType = InteractionType.Viewed
            };

            this.database.EventsRssUserInteraction.Add(eventRssViewed);
            this.database.SaveChanges();
        }

        public void MarkRead(List<long> ids)
        {
            this.database.UsersSubscriptionsToRead
                .Where(x => ids.Contains(x.Id)).ToList()
                .ForEach(x => x.IsRead = true);
            this.database.SaveChanges();
        }

        public void MarkClicked(long id, long currentUserId)
        {
            var rssEntryToRead = this.database.RssEntriesToRead.Single(x => x.Id == id);

            var eventRssClicked = new EventRssUserInteraction()
            {
                UserId = currentUserId,
                RssEntryId = rssEntryToRead.RssEntryId,
                InteractionType = InteractionType.Clicked
            };

            this.database.EventsRssUserInteraction.Add(eventRssClicked);
            this.database.SaveChanges();
        }
    }
}
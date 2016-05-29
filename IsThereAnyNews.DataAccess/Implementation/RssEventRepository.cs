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
            var eventRssViewed = new EventRssViewed
            {
                RssEntryId = rssEntryId,
                UserId = currentUserId
            };

            this.database.EventsRssViewed.Add(eventRssViewed);
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
            var rssEntryToRead = this.database.RssEntriesToRead
                .Where(x => x.Id == id)
                .Single();

            var eventRssClicked = new EventRssClicked()
            {
                UserId = currentUserId,
                RssEntryId = rssEntryToRead.RssEntryId
            };

            this.database.EventsRssClicked.Add(eventRssClicked);
            this.database.SaveChanges();
        }
    }
}
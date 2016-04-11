using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssEventRepository
    {
        void AddEventRssViewed(long currentUserId, long rssToReadId);
    }

    class RssEventRepository : IRssEventRepository
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
    }
}
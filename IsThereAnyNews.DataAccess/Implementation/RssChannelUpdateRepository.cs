using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelUpdateRepository : IRssChannelUpdateRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelUpdateRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveEvent(EventRssChannelUpdated eventRssChannelUpdated)
        {
            this.database.RssChannelUpdates.Add(eventRssChannelUpdated);
            this.database.SaveChanges();
        }
    }
}
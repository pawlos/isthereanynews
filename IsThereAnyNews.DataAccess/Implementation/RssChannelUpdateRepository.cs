namespace IsThereAnyNews.DataAccess.Implementation
{
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Events;

    public class RssChannelUpdateRepository : IRssChannelUpdateRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelUpdateRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveEvent(long eventRssChannelUpdated)
        {
            var rssChannelUpdated = new EventRssChannelUpdated { RssChannelId = eventRssChannelUpdated };
            this.database.RssChannelUpdates.Add(rssChannelUpdated);
            this.database.SaveChanges();
        }
    }
}
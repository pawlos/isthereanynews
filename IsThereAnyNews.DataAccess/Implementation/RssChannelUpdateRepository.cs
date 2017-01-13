namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Linq;

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

        public DateTime GetLatestUpdateDate(long rssChannelId)
        {
            var eventRssChannelUpdated = this.database.RssChannelUpdates.Where(c => c.RssChannelId == rssChannelId)
                .OrderByDescending(c => c.Created)
                .FirstOrDefault();
            return eventRssChannelUpdated?.Created ?? DateTime.MinValue;
        }
    }
}
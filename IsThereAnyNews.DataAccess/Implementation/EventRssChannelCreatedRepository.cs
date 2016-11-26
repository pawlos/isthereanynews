namespace IsThereAnyNews.DataAccess.Implementation
{
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Events;

    public class EventRssChannelCreatedRepository : IEventRssChannelCreatedRepository
    {
        private readonly ItanDatabaseContext database;

        public EventRssChannelCreatedRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(long eventRssChannelCreated)
        {
            var rssChannelCreated = new EventRssChannelCreated { RssChannelId = eventRssChannelCreated };
            this.database.EventRssChannelCreated.Add(rssChannelCreated);
            this.database.SaveChanges();
        }
    }
}
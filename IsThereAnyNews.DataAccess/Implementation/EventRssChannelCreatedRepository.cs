using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class EventRssChannelCreatedRepository : IEventRssChannelCreatedRepository
    {
        private readonly ItanDatabaseContext database;

        public EventRssChannelCreatedRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(EventRssChannelCreated eventRssChannelCreated)
        {
            this.database.EventRssChannelCreated.Add(eventRssChannelCreated);
            this.database.SaveChanges();
        }
    }
}
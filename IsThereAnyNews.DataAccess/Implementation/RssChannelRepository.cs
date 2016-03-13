using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelRepository : IRssChannelRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelRepository() : this(new ItanDatabaseContext())
        {

        }

        public RssChannelRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public RssChannel LoadRssChannel(long id)
        {
            return this.database.RssChannels.Single(x => x.Id == id);
        }
    }
}